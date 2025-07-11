
namespace FifthModJam
{
    public static class Constants
    {
        // Blink time
        public const float BLINK_TIME = 0.5f;
        public const float BLINK_CLOSE_ANIM_TIME = BLINK_TIME / 2f;
        public const float BLINK_OPEN_ANIM_TIME = BLINK_TIME / 2f;
        public const float BLINK_STAY_CLOSED_TIME = 1f;

        // Unity Paths
        public const string UNITYPATH_STARLIGHT = "SilverLining_Body/Sector/Star/StarLight";
        public const string UNITYPATH_MUSEUM = "ScaledMuseum_Body/Sector";
        // Unity Paths - Diorama specific
        public const string UNITYPATH_SCALEDMUSEUM = "ScaledMuseum_Body/Sector/ScaledMuseum";
        public const string UNITYPATH_EXHIBITS_PREFIX = UNITYPATH_SCALEDMUSEUM + "/Offset/Exhibits/";
        public const string UNITYPATH_EXHIBITS_SUFFIX_STR = "Exhibit_STR/Spawn/SpawnKAV1";
        public const string UNITYPATH_EXHIBITS_SUFFIX_NOM = "Exhibit_NOM/Spawn/SpawnKAV2";
        public const string UNITYPATH_EXHIBITS_SUFFIX_HEA = "Exhibit_HEA/Spawn/SpawnKAV3";
        public const string UNITYPATH_EXHIBITS_SUFFIX_KAR = "Exhibit_KAV/Spawn/SpawnKAV4";
        public const string UNITYPATH_KARVISHIP_SPAWNRETURN = "OminousOrbiter_Body/Sector/KarviShip_Interior/Interactibles/SpawnReturn/SpawnKAV5";
        

    }
}
