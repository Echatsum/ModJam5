using UnityEngine;

namespace FifthModJam
{
    // Bit of a dirty way to add a second code to the totem, when all we needed was an edit to the check for code method
    public class EclipseCodeController4Expanded : AbstractGhostDoorInterface
	{
		[SerializeField]
		private InteractReceiver _interactReceiver;

		[SerializeField]
		private GearInterfaceEffects _gearInterfaceHorizontal;

		[SerializeField]
		private GearInterfaceEffects _gearInterfaceVertical;

		[SerializeField]
		private RotaryDial[] _dials;

		[SerializeField]
		private Transform _lockOnTransform;

		[SerializeField]
		private Transform[] _selectors;

		[Space]
		[SerializeField]
		private int[] _code;
		public int[] GetCode() => _code;

		[SerializeField]
		private int[] _secondCode;
		[SerializeField]
		private TotemCodePromptEnum _totemLocation;

		[SerializeField]
		private string[] _factIDs = new string[0];

		[Space]
		[SerializeField]
		private OWAudioSource _oneShotAudio;

		private ScreenPrompt _leftRightPrompt;

		private ScreenPrompt _upDownPrompt;

		private ScreenPrompt _leavePrompt;

		private int _selectedDial;

		private bool _codeCheckDirty;

		private bool _playerInteracting;

		private bool _movingSelector;

		private float _currentSelectorPosY;

		private float _targetSelectorPosY;

		private void Awake()
		{
			if (_interactReceiver != null)
			{
				_interactReceiver.OnPressInteract += OnPressInteract;
			}
			_selectedDial = 0;
		}

		private void Start()
		{
			if (_interactReceiver != null)
			{
				_interactReceiver.SetPromptText(UITextType.UnknownInterfacePrompt);
			}
			_currentSelectorPosY = (_targetSelectorPosY = _dials[_selectedDial].transform.localPosition.y);
			for (int i = 0; i < _selectors.Length; i++)
			{
				_selectors[i].SetLocalPositionY(_currentSelectorPosY);
			}
			_leftRightPrompt = new ScreenPrompt(InputLibrary.left, InputLibrary.right, UITextLibrary.GetString(UITextType.RotateGearLeftRightPrompt) + "   <CMD>", ScreenPrompt.MultiCommandType.POS_NEG);
			_upDownPrompt = new ScreenPrompt(InputLibrary.up, InputLibrary.down, UITextLibrary.GetString(UITextType.RotateGearUpDownPrompt) + "   <CMD>", ScreenPrompt.MultiCommandType.POS_NEG);
			_leavePrompt = new ScreenPrompt(InputLibrary.cancel, UITextLibrary.GetString(UITextType.LeavePrompt) + "   <CMD>");
			base.enabled = false;
		}

		private void OnDestroy()
		{
			if (_interactReceiver != null)
			{
				_interactReceiver.OnPressInteract -= OnPressInteract;
			}
		}

		private bool MoveSelectorToLocalPositionY(float yPos)
		{
			if (OWMath.ApproxEquals(yPos, _targetSelectorPosY, 0.01f))
			{
				return false;
			}
			_oneShotAudio.PlayOneShot(AudioType.CodeTotem_Vertical);
			_targetSelectorPosY = yPos;
			_movingSelector = true;
			return true;
		}

		private void Update()
		{
			if (_movingSelector)
			{
				_currentSelectorPosY = Mathf.MoveTowards(_currentSelectorPosY, _targetSelectorPosY, Time.deltaTime * 1.5f);
				if (OWMath.ApproxEquals(_currentSelectorPosY, _targetSelectorPosY))
				{
					_currentSelectorPosY = _targetSelectorPosY;
					_movingSelector = false;
				}
				for (int i = 0; i < _selectors.Length; i++)
				{
					_selectors[i].SetLocalPositionY(_currentSelectorPosY);
				}
			}
			if (!_playerInteracting && !_movingSelector)
			{
				base.enabled = false;
			}
			bool flag = OWInput.IsInputMode(InputMode.SatelliteCam);
			_leftRightPrompt.SetVisibility(flag);
			_upDownPrompt.SetVisibility(flag);
			_leavePrompt.SetVisibility(flag);
			if (!flag)
			{
				return;
			}
			if ((OWInput.IsNewlyPressed(InputLibrary.right) || OWInput.IsNewlyPressed(InputLibrary.right2) || OWInput.IsNewlyPressed(InputLibrary.toolActionPrimary)) && !_gearInterfaceVertical.IsRotating())
			{
				_dials[_selectedDial].Rotate(positive: true);
				_gearInterfaceHorizontal.AddRotation(-45f, 0f);
				_oneShotAudio.PlayOneShot(AudioType.CodeTotem_Horizontal);
				_codeCheckDirty = true;
			}
			else if ((OWInput.IsNewlyPressed(InputLibrary.left) || OWInput.IsNewlyPressed(InputLibrary.left2) || OWInput.IsNewlyPressed(InputLibrary.toolActionSecondary)) && !_gearInterfaceVertical.IsRotating())
			{
				_dials[_selectedDial].Rotate(positive: false);
				_gearInterfaceHorizontal.AddRotation(45f, 0f);
				_oneShotAudio.PlayOneShot(AudioType.CodeTotem_Horizontal);
				_codeCheckDirty = true;
			}
			else if (OWInput.IsNewlyPressed(InputLibrary.up) || OWInput.IsNewlyPressed(InputLibrary.up2))
			{
				_selectedDial = Mathf.Max(_selectedDial - 1, 0);
				if (MoveSelectorToLocalPositionY(_dials[_selectedDial].transform.localPosition.y))
				{
					_gearInterfaceVertical.AddRotation(45f, 0f);
				}
				else
				{
					_gearInterfaceVertical.PlayFailure(forward: true, 0.5f);
				}
			}
			else if (OWInput.IsNewlyPressed(InputLibrary.down) || OWInput.IsNewlyPressed(InputLibrary.down2))
			{
				_selectedDial = Mathf.Min(_selectedDial + 1, _dials.Length - 1);
				if (MoveSelectorToLocalPositionY(_dials[_selectedDial].transform.localPosition.y))
				{
					_gearInterfaceVertical.AddRotation(-45f, 0f);
				}
				else
				{
					_gearInterfaceVertical.PlayFailure(forward: false);
				}
			}
			else if (OWInput.IsNewlyPressed(InputLibrary.cancel))
			{
				CancelInteraction();
			}
			for (int j = 0; j < _dials.Length; j++)
			{
				if (_dials[j].IsRotating())
				{
					return;
				}
			}
			if (_codeCheckDirty)
			{
				CheckForCode();
				_codeCheckDirty = false;
			}
		}

		private void CheckForCode()
		{
			bool flag = true;
			bool flag2 = true;
			for (int i = 0; i < _dials.Length; i++)
			{
				flag = flag && _dials[i].GetSymbolSelected() == _code[i];
				flag2 = flag2 && _dials[i].GetSymbolSelected() == _secondCode[i];
			}
			if (flag)
			{
				CallOpenEvent();

				// Add perm cond for opening door
				if (_totemLocation == TotemCodePromptEnum.REELHOUSE)
				{
					PlayerData.SetPersistentCondition(Constants.PERMCOND_OPENDOOR_REELHOUSE, state: true);
				}
				else if (_totemLocation == TotemCodePromptEnum.VOLCANOSUMMIT)
				{
					PlayerData.SetPersistentCondition(Constants.PERMCOND_OPENDOOR_VOLCANOSUMMIT, state: true);
				}
				// Check for knockknock achievement
				if (PlayerData.GetPersistentCondition(Constants.PERMCOND_OPENDOOR_REELHOUSE) && PlayerData.GetPersistentCondition(Constants.PERMCOND_OPENDOOR_VOLCANOSUMMIT))
				{
					FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_KNOCK_KNOCK);
				}
			}
			else
			{
				CallCloseEvent();
			}
            if (flag2)
			{
				// Add perm cond for mixed password on this door
				if (_totemLocation == TotemCodePromptEnum.REELHOUSE)
				{
					PlayerData.SetPersistentCondition(Constants.PERMCOND_MIXEDPASSWORD_REELHOUSE, state: true);
				}
				else if (_totemLocation == TotemCodePromptEnum.VOLCANOSUMMIT)
				{
					PlayerData.SetPersistentCondition(Constants.PERMCOND_MIXEDPASSWORD_VOLCANOSUMMIT, state: true);
				}
				// Check for mixedpassword achievement
				if (PlayerData.GetPersistentCondition(Constants.PERMCOND_MIXEDPASSWORD_REELHOUSE) && PlayerData.GetPersistentCondition(Constants.PERMCOND_MIXEDPASSWORD_VOLCANOSUMMIT))
				{
					FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_MIXED_PASSWORDS);
				}
			}
		}

		private void OnPressInteract()
		{
			Locator.GetToolModeSwapper().UnequipTool();
			Locator.GetPlayerTransform().GetComponent<PlayerLockOnTargeting>().LockOn(_lockOnTransform, Vector3.zero);
			GlobalMessenger.FireEvent("EnterSatelliteCameraMode");
			Locator.GetPromptManager().AddScreenPrompt(_upDownPrompt, PromptPosition.UpperRight);
			Locator.GetPromptManager().AddScreenPrompt(_leftRightPrompt, PromptPosition.UpperRight);
			Locator.GetPromptManager().AddScreenPrompt(_leavePrompt, PromptPosition.UpperRight);
			for (int i = 0; i < _factIDs.Length; i++)
			{
				Locator.GetShipLogManager().RevealFact(_factIDs[i]);
			}
			base.enabled = true;
			_playerInteracting = true;
		}

		private void CancelInteraction()
		{
			Locator.GetPromptManager().RemoveScreenPrompt(_leftRightPrompt);
			Locator.GetPromptManager().RemoveScreenPrompt(_upDownPrompt);
			Locator.GetPromptManager().RemoveScreenPrompt(_leavePrompt);
			Locator.GetPlayerTransform().GetComponent<PlayerLockOnTargeting>().BreakLock();
			_interactReceiver.ResetInteraction();
			GlobalMessenger.FireEvent("ExitSatelliteCameraMode");
			_playerInteracting = false;
		}

		public override void SetStartingPosition(bool IsActivated)
		{
		}
	}
}
