using Microsoft.Maui.Controls;

namespace randomWordGenerator
{
    internal class ScrabbleBoard
    {
        public Grid BoardGrid { get; private set; }
        public event EventHandler? ClearChoosenLetterRequested;
        public string LetterToPut = "";
        private bool isFirstLetterPlaced;
        private bool isSecondLetterPlaced;
        private bool isVertical;
        private bool isHorizontal;
        private int[] firstCoords = new int[2];
        private int[] lastCoords = new int[2];

        public ScrabbleBoard()
        {
            BoardGrid = new Grid();
            CreateScrabbleBoard();
        }

        private void CreateScrabbleBoard()
        {
            for (int i = 0; i < 15; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    var frame = new Frame
                    {
                        BorderColor = Colors.Black,
                        BackgroundColor = Colors.LightGray,
                        WidthRequest = 30,
                        HeightRequest = 30,
                        HasShadow = false,
                        Padding = 0
                    };

                    var pointerEnteredRecognizer = new PointerGestureRecognizer();
                    pointerEnteredRecognizer.PointerEntered += OnPointerEntered;
                    frame.GestureRecognizers.Add(pointerEnteredRecognizer);

                    var pointerExitedRecognizer = new PointerGestureRecognizer();
                    pointerExitedRecognizer.PointerExited += OnPointerExited;
                    frame.GestureRecognizers.Add(pointerExitedRecognizer);

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += OnFrameTapped;
                    frame.GestureRecognizers.Add(tapGestureRecognizer);

                    BoardGrid.Children.Add(frame);
                    Grid.SetRow(frame, row);
                    Grid.SetColumn(frame, col);
                }
            }
        }

        private void OnFrameTapped(object? sender, EventArgs e)
        {
            if (sender is Frame frame && !string.IsNullOrEmpty(LetterToPut))
            {
                int row = Grid.GetRow(frame);
                int col = Grid.GetColumn(frame);

                if (!isFirstLetterPlaced || IsAdjacentToExistingLetter(row, col))
                {
                    frame.Content = new Label
                    {
                        Text = LetterToPut,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black,
                        FontSize = 20,
                    };
                    frame.Background = Colors.Green;

                    LetterToPut = "";
                    ClearChoosenLetterRequested?.Invoke(this, EventArgs.Empty);

                    if (!isFirstLetterPlaced)
                    {
                        firstCoords[0] = lastCoords[0] = row;
                        firstCoords[1] = lastCoords[1] = col;
                        isHorizontal = true;
                        isVertical = true;
                        ColoringFields();
                        isHorizontal = false;
                        isVertical = false;
                        isFirstLetterPlaced = true;
                    }
                    else
                    {
                        UpdateCoords(row, col);
                    }
                }
            }
        }

        private void UpdateCoords(int row, int col)
        {
            if (!isSecondLetterPlaced)
            {
                isSecondLetterPlaced = true;
                isVertical = IsFrameOccupied(row - 1, col) || IsFrameOccupied(row + 1, col);
                isHorizontal = IsFrameOccupied(row, col - 1) || IsFrameOccupied(row, col + 1);
            }

            if (isVertical)
            {
                if (row < firstCoords[0])
                {
                    firstCoords[0] = row;
                    firstCoords[1] = col;
                }
                else if (row > lastCoords[0])
                {
                    lastCoords[0] = row;
                    lastCoords[1] = col;
                }
            }
            else if (isHorizontal)
            {
                if (col < firstCoords[1])
                {
                    firstCoords[0] = row;
                    firstCoords[1] = col;
                }
                else if (col > lastCoords[1])
                {
                    lastCoords[0] = row;
                    lastCoords[1] = col;
                }
            }

            ColoringFields();
        }

        public void ColoringFields()
        {
            ResetColoredFrames();

            if (isVertical)
            {
                ColorFrame(firstCoords[0] - 1, firstCoords[1]);
                ColorFrame(lastCoords[0] + 1, lastCoords[1]);
            }
            if (isHorizontal)
            {
                ColorFrame(firstCoords[0], firstCoords[1] - 1);
                ColorFrame(lastCoords[0], lastCoords[1] + 1);
            }
        }

        private void ResetColoredFrames()
        {
            foreach (var child in BoardGrid.Children)
            {
                if (child is Frame frame && frame.BackgroundColor == Colors.LightGreen)
                {
                    frame.BackgroundColor = Colors.LightGray;
                }
            }
        }

        private void ColorFrame(int row, int col)
        {
            if (row >= 0 && row < 15 && col >= 0 && col < 15)
            {
                var frame = BoardGrid.Children
                    .FirstOrDefault(c => Grid.GetRow((BindableObject)c) == row && Grid.GetColumn((BindableObject)c) == col) as Frame;
                if (frame != null)
                {
                    frame.BackgroundColor = Colors.LightGreen;
                }
            }
        }


        private bool IsAdjacentToExistingLetter(int row, int col)
        {
            if (isSecondLetterPlaced)
            {
                return isVertical
                    ? IsFrameOccupied(row - 1, col) || IsFrameOccupied(row + 1, col)
                    : IsFrameOccupied(row, col - 1) || IsFrameOccupied(row, col + 1);
            }
            return IsFrameOccupied(row - 1, col) || IsFrameOccupied(row + 1, col) ||
                   IsFrameOccupied(row, col - 1) || IsFrameOccupied(row, col + 1);
        }

        private bool IsFrameOccupied(int row, int col)
        {
            if (row >= 0 && row < 15 && col >= 0 && col < 15)
            {
                var frame = BoardGrid.Children
                    .FirstOrDefault(c => Grid.GetRow((BindableObject)c) == row && Grid.GetColumn((BindableObject)c) == col) as Frame;
                return frame != null && frame.Content is Label;
            }
            return false;
        }

        private void OnPointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is Frame frame && frame.BackgroundColor != Colors.Green && frame.BackgroundColor != Colors.LightGreen)
            {
                frame.BackgroundColor = Colors.Blue;
            }
        }

        private void OnPointerExited(object? sender, PointerEventArgs e)
        {
            if (sender is Frame frame && frame.BackgroundColor != Colors.Green && frame.BackgroundColor != Colors.LightGreen)
            {
                frame.BackgroundColor = Colors.LightGray;
            }
        }
    }
}
