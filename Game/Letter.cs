﻿namespace randomWordGenerator.Game;

public class Letter
{
    public Letter(char character, int quantity, int points)
    {
        Character = character;
        Quantity = quantity;
        Points = points;
    }

    public char Character { get; set; }
    public int Quantity { get; set; }
    public int Points { get; set; }
}