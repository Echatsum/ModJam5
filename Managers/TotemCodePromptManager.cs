using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Manager made to give the prompt data for the totems.
    /// </summary>
    public class TotemCodePromptManager : AbstractManager<TotemCodePromptManager>
    {
        private enum OwlkSymbolEnum
        {
            INVALID = -1,

            CRESCENT = 0,
            HALFMOON = 1,
            GIBBOUS = 2,
            RING = 3,
            SUN = 4,
            STAR = 5,
            EYE = 6,
            FIRE = 7
        }
        private readonly Dictionary<int, OwlkSymbolEnum> _conversionDict = new()
        {
            { 0, OwlkSymbolEnum.CRESCENT },
            { 1, OwlkSymbolEnum.HALFMOON },
            { 2, OwlkSymbolEnum.GIBBOUS },
            { 3, OwlkSymbolEnum.RING },
            { 4, OwlkSymbolEnum.SUN },
            { 5, OwlkSymbolEnum.STAR },
            { 6, OwlkSymbolEnum.EYE },
            { 7, OwlkSymbolEnum.FIRE },
        };

        private readonly Dictionary<TotemCodePromptEnum, Tuple<string, IList<string>>> _dictPrompt = new(); // Enum Key - prompt text - fact requirements

        public void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return; // [Note: this sends an error anyway because NewHorizonsAPI might not be ready]

            // Reel House
            var promptText = FifthModJam.NewHorizonsAPI.GetTranslationForUI("$COSMICCURATORS_TOTEM_PROMPT_REELHOUSE_TEXT");
            var factRequirements = new List<string> { "COSMICCURATORS_REEL_HOUSE_ENTER" };
            _dictPrompt.Add(TotemCodePromptEnum.REELHOUSE, new Tuple<string, IList<string>>(promptText, factRequirements));

            // Volcano Summit
            promptText = FifthModJam.NewHorizonsAPI.GetTranslationForUI("$COSMICCURATORS_TOTEM_PROMPT_VOLCANOSUMMIT_TEXT");
            factRequirements = new List<string> { "COSMICCURATORS_VOLCANO_SUMMIT_R" };
            _dictPrompt.Add(TotemCodePromptEnum.VOLCANOSUMMIT, new Tuple<string, IList<string>>(promptText, factRequirements));

            FifthModJam.WriteLineReady("TotemCodePromptManager");
        }

        public string GetPromptText(TotemCodePromptEnum key)
        {
            return _dictPrompt.ContainsKey(key) ? _dictPrompt[key].Item1 : "INVALID PROMPT KEY";
        }
        public bool DoesMeetPromptRequirements(TotemCodePromptEnum key)
        {
            if (!_dictPrompt.ContainsKey(key)) return false; // invalid key

            // Check if all facts are revealed
            var list = _dictPrompt[key].Item2;
            foreach (var fact in list)
            {
                if (!Locator.GetShipLogManager().IsFactRevealed(fact))
                {
                    return false;
                }
            }
            return true;
        }

        public Sprite GetSprite(IList<int> list)
        {
            var realList = ConvertIntToEnum(list);
            var texture = MakeTexture(realList);

            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2f, texture.height / 2f), 100, 0, SpriteMeshType.FullRect, Vector4.zero, false);

            return sprite;
        }
        private IList<OwlkSymbolEnum> ConvertIntToEnum(IList<int> list)
        {
            if (list == null) return null;

            var newList = new List<OwlkSymbolEnum>();
            for (int i = 0; i < list.Count; i++)
            {
                var key = list[i];
                OwlkSymbolEnum val = _conversionDict.ContainsKey(key) ? _conversionDict[key] : OwlkSymbolEnum.INVALID;
                newList.Add(val);
            }
            return newList;
        }
        private Texture2D MakeTexture(IList<OwlkSymbolEnum> list)
        {
            if (list == null || list.Count == 0) return null; // Invalid list

            // Prep the texture
            int width = 96;
            int height = 96;
            Texture2D texture = new Texture2D(width * list.Count, height, TextureFormat.RGBA32, false, false);
            texture.SetPixels(Enumerable.Repeat(Color.clear, texture.width * texture.height).ToArray());

            // Add symbols
            float offset = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                var symbol = list[i];
                if (symbol == OwlkSymbolEnum.INVALID) continue; // Invalid value

                //Rect rect = new Rect(offset, 0f, width, height);
                // TODO: Add the symbol decal to the texture here..

                // ..
                offset += width;
            }
            texture.Apply();
            return texture;
        }
    }
}
