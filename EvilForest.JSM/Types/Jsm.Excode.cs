namespace FF8.JSM
{
    public static partial class Jsm
    {
        public enum Excode
        {
            B_PAD0 = 0,
            B_PAD1 = 1,
            B_PAD2 = 2,
            B_PAD3 = 3,
            IncrementPost = 4,
            DecrementPost = 5,
            B_PRE_PLUS = 6,
            B_PRE_MINUS = 7,
            B_POST_PLUS_A = 8,
            B_POST_MINUS_A = 9,
            B_PRE_PLUS_A = 10,
            B_PRE_MINUS_A = 11,
            B_SINGLE_PLUS = 12,
            B_SINGLE_MINUS = 13,
            B_NOT = 14,
            B_NOT_E = 15,
            
            /// <summary>
            /// Bitwise NOT operator.
            /// </summary>
            B_COMP = 16,
            
            Mul = 17,
            Div = 18,
            Mod = 19,
            Add = 20,
            Sub = 21,
            BitLeft = 22,
            BitRight = 23,
            LessThan = 24,
            GreatThan = 25,
            LessOrEquals = 26,
            GreatOrEquals = 27,
            LessThanList = 28,
            GreatThanList = 29,
            LessOrEqualsList = 30,
            GreatOrEqualsList = 31,
            Equivalence = 32,
            NotEquivalence = 33,
            B_EQ_E = 34,
            B_NE_E = 35,
            BitAnd = 36,
            BitXor = 37,
            BitOr = 38,
            BooleanAnd = 39,
            BooleanOr = 40,
            B_MEMBER = 41,
            B_COUNT = 42,
            B_PICK = 43,
            Let = 44,
            B_LET_A = 45,
            B_LET_E = 46,
            MulLet = 47,
            DivLet = 48,
            ModeLet = 49,
            AddLet = 50,
            SubLet = 51,
            BitLeftLet = 52,
            BitRightLet = 53,
            B_MULT_LET_A = 54,
            B_DIV_LET_A = 55,
            B_REM_LET_A = 56,
            B_PLUS_LET_A = 57,
            B_MINUS_LET_A = 58,
            B_SHIFT_LEFT_LET_A = 59,
            B_SHIFT_RIGHT_LET_A = 60,
            BitAndLet = 61,
            BitXorLet = 62,
            BitOrLet = 63,
            B_AND_LET_A = 64,
            B_XOR_LET_A = 65,
            B_OR_LET_A = 66,
            B_AND_LET_E = 67,
            B_XOR_LET_E = 68,
            B_OR_LET_E = 69,
            B_CAST8 = 70,
            B_CAST8U = 71,
            B_CAST16 = 72,
            B_CAST16U = 73,
            B_CAST_LIST = 74,
            B_LMAX = 75,
            B_LMIN = 76,
            B_SELECT = 77,
            B_OBJSPEC = 78,

            /// <summary>
            /// Check if a button of the controller was pressed right before the current frame.
            /// </summary>
            IsKeyPressed = 79,
            B_SIN2 = 80,
            B_COS2 = 81,
            GetCurrentHP = 82,
            GetMaxHP = 83,
            B_AND_E = 84,
            B_NAND_E = 85,
            B_XOR_E = 86,
            B_OR_E = 87,

            /// <summary>
            /// Check if a button of the controller was released right before the current frame.
            /// </summary>
            B_KEYOFF = 88,

            /// <summary>
            /// Check if a button of the controller is currently pressed.
            /// </summary>
            B_KEY = 89,

            /// <summary>
            /// A button pressing check.
            /// </summary>
            B_KEYON2 = 90,

            /// <summary>
            /// A button releasing check.
            /// </summary>
            B_KEYOFF2 = 91,

            /// <summary>
            /// A button holding check.
            /// </summary>
            B_KEY2 = 92,
            
            GetActorAngle = 93,
            GetActorDistance = 94,

            /// <summary>
            /// Retrieve an object's UID or 0 if not found.
            /// Only useful for checking if an object exists with the UID or convert special entry IDs (250 to 255) to a valid UID.
            /// </summary>
            B_PTR = 95,

            /// <summary>
            /// An angle calculation.
            /// </summary>
            B_ANGLEA = 96,

            /// <summary>
            /// A distance calculation.
            /// </summary>
            B_DISTANCEA = 97,

            /// <summary>
            /// Calculate the sinus of an angle and multiply it by 4096 to get an integer.
            /// </summary>
            B_SIN = 98,

            /// <summary>
            /// Calculate the cosinus of an angle and multiply it by 4096 to get an integer.
            /// </summary>
            B_COS = 99,

            /// <summary>
            /// Return the amount of items of an item type in the player's inventory.
            /// </summary>
            B_HAVE_ITEM = 100,
            B_BAFRAME = 101,
            GetAngle = 102,
            pad67 = 103,
            pad68 = 104,
            pad69 = 105,

            /// <summary>
            /// Get character animation frame
            /// </summary>
            B_FRAME = 106,

            /// <summary>
            /// Check if a character is in the player's party.
            /// </summary>
            IsInParty = 107,
            B_SPS = 108,

            /// <summary>
            /// Add a character to the player's party.
            /// </summary>
            AddToParty = 109,
            GetCurrentMP = 110,
            GetMaxMP = 111,
            B_BGIID = 112,
            B_BGIFLOOR = 113,
            B_OBJSPECA = 120,
            B_SYSLIST = 121,
            B_SYSVAR = 122,
            B_pad7b = 123,
            B_PAD4 = 124,
            Const16 = 125,
            Const26 = 126,
            End = 127,
            B_VAR = 192
        }
    }
}