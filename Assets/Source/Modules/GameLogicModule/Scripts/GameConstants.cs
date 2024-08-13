using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts
{
    public class GameConstants //calibrating depends on preferences of screen size, gameplay field, difficulty etc.
    {
        public const int MIN_LETTERS_COUNT = 4;
        public const int MAX_LETTERS_COUNT = 7;

        public const int MAX_WORDS_COUNT_PER_BLOCK = 6;
        public const int MAX_GAME_FIELD_SIZE_LETTERS = 22; //4-5-6-7 letters words blocks
    }
}
