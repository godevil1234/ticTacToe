using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ticTacToe;

public partial class MainWindow : Window
{
    private string[,] board = new string[3, 3];
    private bool isPlayerX = true;
    private bool isPlayingWithComputer = false;
    private Random random = new Random();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartGame(object sender, RoutedEventArgs e)
    {
        isPlayingWithComputer = true;
        MainMenu.Visibility = Visibility.Collapsed;
        GameBoard.Visibility = Visibility.Visible;
        ResetBoard();

        isPlayerX = false;
        ComputerMove();
    } //

    private void StartGameWithPlayer(object sender, RoutedEventArgs e)
    {
        isPlayingWithComputer = false;
        MainMenu.Visibility = Visibility.Collapsed;
        GameBoard.Visibility = Visibility.Visible;
        ResetBoard();
    }

    private void ResetBoard()
    {
        foreach (var child in GameBoard.Children)
        {
            if (child is Button btn)
            {
                btn.Content = "";
                btn.IsEnabled = true;
            }
        }
        board = new string[3, 3];
        isPlayerX = true;
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && string.IsNullOrEmpty(btn.Content?.ToString()))
        {
            btn.Content = isPlayerX ? "X" : "O";
            btn.IsEnabled = false;

            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);
            board[row, col] = isPlayerX ? "X" : "O";

            if (CheckWinner(isPlayerX ? "X" : "O"))
            {
                MessageBox.Show($"Игрок {btn.Content} победил!");
                ResetBoard();
                return;
            }

            if (IsBoardFull())
            {
                MessageBox.Show("Ничья!");
                ResetBoard();
                return;
            }

            if (isPlayingWithComputer)
            {
                isPlayerX = false;
                ComputerMove();
            }
            else
            {
                isPlayerX = !isPlayerX;
            }
        }
    }

    private void ComputerMove()
    {
        var emptyCells = GameBoard.Children.OfType<Button>()
            .Where(btn => string.IsNullOrEmpty(btn.Content?.ToString()))
            .ToList();

        if (emptyCells.Count > 0)
        {
            var btn = emptyCells[random.Next(emptyCells.Count)];
            btn.Content = "O";
            btn.IsEnabled = false;

            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);
            board[row, col] = "O";

            if (CheckWinner("O"))
            {
                MessageBox.Show("Компьютер победил!");
                ResetBoard();
            }
            else
            {
                if (IsBoardFull())
                {
                    MessageBox.Show("Ничья!");
                    ResetBoard();
                }
                else
                {
                    isPlayerX = true;
                }
            }
        }
    }

    private bool CheckWinner(string player)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
                (board[0, i] == player && board[1, i] == player && board[2, i] == player))
                return true;
        }

        if ((board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
            (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player))
            return true;

        return false;
    }

    private bool IsBoardFull()
    {
        foreach (var cell in board)
        {
            if (string.IsNullOrEmpty(cell)) return false;
        }
        return true;
    }
}