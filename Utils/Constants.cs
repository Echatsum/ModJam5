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


        // Achievements
        public const string ACHIEVEMENT_SHRUNK_HATCHLING = "COSMICCURATORS.SHRUNK_HATCHLING";
        public const string ACHIEVEMENT_WHATS_THIS_BUTTON = "COSMICCURATORS.WHATS_THIS_BUTTON";
        public const string ACHIEVEMENT_KNOCK_KNOCK = "COSMICCURATORS.KNOCK_KNOCK";
        public const string ACHIEVEMENT_THE_COSMIC_CURATORS = "COSMICCURATORS.THE_COSMIC_CURATORS";

        public const string ACHIEVEMENT_ERNESTO = "COSMICCURATORS.ERNESTO";
        public const string ACHIEVEMENT_ONE_RING_TO_RULE_THEM_ALL = "COSMICCURATORS.ONE_RING_TO_RULE_THEM_ALL";
        public const string ACHIEVEMENT_INFINITY_STICK = "COSMICCURATORS.INFINITY_STICK";
        public const string ACHIEVEMENT_SCOUTLESS = "COSMICCURATORS.SCOUTLESS";
        public const string ACHIEVEMENT_MIXED_PASSWORDS = "COSMICCURATORS.MIXED_PASSWORDS";
        public const string ACHIEVEMENT_WALK_THE_PLANK = "COSMICCURATORS.WALK_THE_PLANK";
        public const string ACHIEVEMENT_ITS_ONLY_A_MODEL = "COSMICCURATORS.ITS_ONLY_A_MODEL";
        public const string ACHIEVEMENT_YOU_FOUND_US = "COSMICCURATORS.YOU_FOUND_US";
        public const string ACHIEVEMENT_ALL_IN_ITS_PLACE = "COSMICCURATORS.ALL_IN_ITS_PLACE";
        public const string ACHIEVEMENT_FAT_SHAMING = "COSMICCURATORS.FAT_SHAMING";

        // Conditions (Perm or Loop)
        public const string PERMCOND_OPENDOOR_REELHOUSE = "CosmicCurators_OpenDoor_ReelHouse_PERM";
        public const string PERMCOND_OPENDOOR_VOLCANOSUMMIT = "CosmicCurators_OpenDoor_VolcanoSummit_PERM";
        public const string PERMCOND_MIXEDPASSWORD_REELHOUSE = "CosmicCurators_MixedPassword_ReelHouse_PERM";
        public const string PERMCOND_MIXEDPASSWORD_VOLCANOSUMMIT = "CosmicCurators_MixedPassword_VolcanoSummit_PERM";

        // Translation keys
        public const string TRANSLATIONKEY_NPCNAME_PHOSPHORUS = "COSMICCURATORS_DIALOGUE_PHOPHORUS_NAME";
    }
}
