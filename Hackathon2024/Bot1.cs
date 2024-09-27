﻿using Hackathon2024.DTO;

namespace Hackathon2024 ;

    public sealed class Bot1 : IBot
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string BotName { get; }

        public Bot1(string botName)
        {
            BotName = botName;
        }

        public char CalculateNextStep(Round round)
        {
            char selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
            while (round.guessed!.Contains(selected))
            {
                selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
            }
            return selected;
        }

        public int Complete(Result result)
        {
            return 0;
        }
    }