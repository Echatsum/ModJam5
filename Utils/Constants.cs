namespace FifthModJam
{
    public static class Constants
    {
        // Blink time
        public const float BLINK_TIME = 0.5f;
        public const float BLINK_CLOSE_ANIM_TIME = BLINK_TIME / 2f;
        public const float BLINK_OPEN_ANIM_TIME = BLINK_TIME / 2f;
        public const float BLINK_STAY_CLOSED_TIME = 1f;

        // Astral bodies
        public const string ASTRALBODY_SILVERLINING = "COSMICCURATORS_ASTRALBODY_SILVER_LINING";
        public const string ASTRALBODY_OMINOUS_ORBITER = "COSMICCURATORS_ASTRALBODY_OMINOUS_ORBITER";
        public const string ASTRALBODY_SCALED_MUSEUM = "COSMICCURATORS_ASTRALBODY_SCALED_MUSEUM";

        // Unity Paths
        public const string UNITYPATH_STARLIGHT = Constants.ASTRALBODY_SILVERLINING + "_Body/Sector/Star/StarLight";
        public const string UNITYPATH_MUSEUM = Constants.ASTRALBODY_SCALED_MUSEUM + "_Body/Sector";
        public const string UNITYPATH_PINGCAMPFIRE = "StarshipCommunity_Body/Sector/NomaiFirepit/";
        // Unity Paths - Diorama specific
        public const string UNITYPATH_SCALEDMUSEUM = Constants.UNITYPATH_MUSEUM + "/ScaledMuseum";
        public const string UNITYPATH_EXHIBITS_PREFIX = Constants.UNITYPATH_SCALEDMUSEUM + "/Offset/Exhibits/";
        public const string UNITYPATH_EXHIBITS_SUFFIX_STR = "Exhibit_STR/Spawn/SpawnKAV1";
        public const string UNITYPATH_EXHIBITS_SUFFIX_NOM = "Exhibit_NOM/Spawn/SpawnKAV2";
        public const string UNITYPATH_EXHIBITS_SUFFIX_HEA = "Exhibit_HEA/Spawn/SpawnKAV3";
        public const string UNITYPATH_EXHIBITS_SUFFIX_KAR = "Exhibit_KAV/Spawn/SpawnKAV4";
        public const string UNITYPATH_KARVISHIP_SPAWNRETURN = Constants.ASTRALBODY_OMINOUS_ORBITER + "_Body/Sector/KarviShip_Interior/Interactibles/SpawnReturn/SpawnKAV5";
        

    }
}
