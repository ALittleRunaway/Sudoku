using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        Dictionary<string, string> for_check = new Dictionary<string, string>();

        public bool Processing()
        {
            Random r = new Random();
            int number;
            char row, column, group;
            Dictionary<char, string> columns = new Dictionary<char, string>();
            Dictionary<char, string> rows = new Dictionary<char, string>();
            Dictionary<char, string> groups = new Dictionary<char, string>();

            rows.Add('0', "");
            rows.Add('1', "");
            rows.Add('2', "");
            rows.Add('3', "");
            rows.Add('4', "");
            rows.Add('5', "");
            rows.Add('6', "");
            rows.Add('7', "");
            rows.Add('8', "");

            columns.Add('0', "");
            columns.Add('1', "");
            columns.Add('2', "");
            columns.Add('3', "");
            columns.Add('4', "");
            columns.Add('5', "");
            columns.Add('6', "");
            columns.Add('7', "");
            columns.Add('8', "");

            groups.Add('0', "");
            groups.Add('1', "");
            groups.Add('2', "");
            groups.Add('3', "");
            groups.Add('4', "");
            groups.Add('5', "");
            groups.Add('6', "");
            groups.Add('7', "");
            groups.Add('8', "");

            
            // go through all of the textboxes
            foreach (UIElement c in grid.Children)
            {
                
                bool another = true;
                if (c is TextBox)
                {
                    string name = ((TextBox)c).Name;
                    row = Convert.ToChar(name[1]);
                    column = Convert.ToChar(name[3]);
                    group = Convert.ToChar(name[5]);
                    int[] available = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    while (another)
                    {
                        try
                        {
                            number = available[r.Next(0, available.Length)];
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            return true;
                        }

                        int numIdx = Array.IndexOf(available, number);
                        List<int> tmp = new List<int>(available);
                        tmp.RemoveAt(numIdx);
                        available = tmp.ToArray();


                        if (rows[row].Contains(number.ToString()) == false)
                        {
                            if (columns[column].Contains(number.ToString()) == false)
                            {
                                if (groups[group].Contains(number.ToString()) == false)
                                {
                                    rows[row] += number.ToString();
                                    columns[column] += number.ToString();
                                    groups[group] += number.ToString();
                                    another = false;

                                    int write = r.Next(0, 2);
                                    if (write == 1) 
                                    { 
                                        ((TextBox)c).Text = number.ToString();
                                        ((TextBox)c).Background = Brushes.AliceBlue;
                                        ((TextBox)c).IsReadOnly = true;
                                    }
                                    else 
                                    { 
                                        for_check.Add(name, number.ToString());
                                        ((TextBox)c).Text = "";
                                        ((TextBox)c).Background = Brushes.White;
                                        ((TextBox)c).IsReadOnly = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void ProcessingCircle()
        {
            MessageBox.Show("Генерация может занять некоторе время. Ожидайте.", "Внимание!");
            bool cont = true;
            while (cont)
            {
                for_check.Clear();
                cont = Processing();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ProcessingCircle();
        }
        
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            bool victory = true;
            foreach (UIElement c in grid.Children)
            {
                if (c is TextBox)
                {
                    if (for_check.ContainsKey(((TextBox)c).Name))
                    {
                        if (((TextBox)c).Text != for_check[((TextBox)c).Name])
                        {
                            victory = false;
                            ((TextBox)c).Text = "";
                        }
                    }
                }
            }
            if (victory == true) { MessageBox.Show("Поздравляем, вы сделали это!", "Ура!"); }
        }
        private void New(object sender, RoutedEventArgs e)
        {
            ProcessingCircle();
        }
    }
}
