using Microsoft.Maui.Controls;

namespace randomWordGenerator
{
    internal class ScrabbleBoard
    {
        public Grid BoardGrid { get; private set; }
        public event EventHandler ClearChoosenLetterRequested;
        public string LetterToPut = "";
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

        private void OnFrameTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame)
            {
                frame.Content = new Label
                {
                    Text = LetterToPut,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black,
                    FontSize = 20
                };
            }

            LetterToPut = "";
            ClearChoosenLetterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                frame.BackgroundColor = Colors.Blue;
            }
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                frame.BackgroundColor = Colors.LightGray;
            }
        }
    }
}
