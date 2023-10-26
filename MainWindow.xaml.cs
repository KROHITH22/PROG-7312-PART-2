using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryCallNumberSorter
{
    public partial class MainWindow : Window
    {
        private List<string> callNumbers;

        public MainWindow()
        {
            InitializeComponent();
            CallNumberListBox.Visibility = Visibility.Hidden;
            UserInputListBox.Visibility = Visibility.Hidden;
            CheckOrderButton.Visibility = Visibility.Hidden;

        
        }

        
        private void ReplacingBooks_Click(object sender, RoutedEventArgs e)
        {
            // Enable the sorting task and disable other tasks
            // You can implement gamification features here as well.
            EnableSortingTask();

            // Generate 10 random call numbers
            callNumbers = GenerateRandomCallNumbers(10);

            // Display the call numbers to the user
            DisplayCallNumbers();

            CallNumberListBox.Visibility= Visibility.Visible;
            UserInputListBox.Visibility= Visibility.Visible;
            CheckOrderButton.Visibility= Visibility.Visible;
        }

        private void EnableSortingTask()
        {
            // Enable sorting task button and disable others
            foreach (UIElement element in rb_stackpanel.Children)
            {
                if (element is Button button)
                {
                    if (button.Content.ToString() == "Replacing Books")
                    {
                        button.IsEnabled = false;
                    }
                    else
                    {
                        button.IsEnabled = true;
                    }
                }
            }
            UserInputListBox.Items.Clear();
            
        }

        private List<string> GenerateRandomCallNumbers(int count)
        {
            List<string> randomCallNumbers = new List<string>();
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int topicNumber = rand.Next(1000, 9999);
                string authorLetters = GetRandomLetters(3);

                randomCallNumbers.Add($"{topicNumber / 100}.{topicNumber % 100} {authorLetters}");
            }

            return randomCallNumbers;
        }

        private string GetRandomLetters(int count)
        {
            Random rand = new Random();
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(alphabet, count)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private void DisplayCallNumbers()
        {
            CallNumberListBox.ItemsSource = callNumbers;
        }

        

        private bool IsCorrectOrder(List<string> userOrderedCallNumbers)
        {
            // Create a sorted copy of the generated call numbers
            List<string> sortedCallNumbers = callNumbers.OrderBy(cn => cn).ToList();

            // Check if the user's order matches the sorted order
            return userOrderedCallNumbers.SequenceEqual(sortedCallNumbers);
        }

        private string _draggedItem;

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            var item = ((FrameworkElement)e.OriginalSource).DataContext as string;
            if (item != null)
            {
                _draggedItem = item;
                DragDrop.DoDragDrop(listBox, _draggedItem, DragDropEffects.Move);
            }
        }
        private void ListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (_draggedItem != null)
            {
                var listBox = sender as ListBox;

                // Remove the dragged item from BOTH list boxes
                CallNumberListBox.Items.Remove(_draggedItem);
                UserInputListBox.Items.Remove(_draggedItem);

                // add to the target list box
                listBox.Items.Add(_draggedItem);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserInputListBox.Items.Count < 10)
            {
                MessageBox.Show("Please drag all numbers across and arrange them.");
                return;
            }

            List<string> userInputList = new List<string>();
            foreach (var item in UserInputListBox.Items)
            {
                userInputList.Add(item.ToString());
            }

            // Sort the list
            userInputList.Sort();
            int wrongCount = 0;

            for (int i = 0; i < UserInputListBox.Items.Count; i++)
            {
                if (!UserInputListBox.Items[i].ToString().Equals(userInputList[i]))
                {
                    wrongCount++;
                }
            }

            if (wrongCount == 0)
            {
                MessageBox.Show("Correct!", "Congratulations");
            }
            else
            {
                MessageBox.Show($"{wrongCount} call numbers are out of order");
            }



namespace LibraryCallNumberSorter
{
    public partial class MainWindow : Window
    {
        private List<string> callNumbers;
        private List<CallNumberDescriptionPair> callNumberDescriptionPairs;
        private int currentQuestionIndex = 0;

        public MainWindow()
        {

            // Initialize the list of call number and description pairs
            callNumberDescriptionPairs = InitializeCallNumberDescriptionPairs();
        }

        private List<CallNumberDescriptionPair> InitializeCallNumberDescriptionPairs()
        {
            // You can replace this with your actual data source
            // For demonstration purposes, we're using some sample data
            return new List<CallNumberDescriptionPair>
            {
                new CallNumberDescriptionPair("QA123", "Mathematics"),
                new CallNumberDescriptionPair("PS456", "English Literature"),
                new CallNumberDescriptionPair("HV789", "Social Sciences"),
                new CallNumberDescriptionPair("QP112", "Biology"),
                // Add more pairs here
            };
        }

        // ... Existing code ...

        private void IdentifyingAreas_Click(object sender, RoutedEventArgs e)
        {
            // Enable the identifying areas task and disable other tasks
            EnableIdentifyingAreasTask();

            // Start the identifying areas task
            StartIdentifyingAreasTask();
        }

        private void EnableIdentifyingAreasTask()
        {
            // Enable identifying areas task button and disable others
            foreach (UIElement element in rb_stackpanel.Children)
            {
                if (element is Button button)
                {
                    if (button.Content.ToString() == "Identifying Areas")
                    {
                        button.IsEnabled = false;
                    }
                    else
                    {
                        button.IsEnabled = true;
                    }
                }
            }
        }

        private void StartIdentifyingAreasTask()
        {
            if (currentQuestionIndex < callNumberDescriptionPairs.Count)
            {
                // Display the next question
                DisplayIdentifyingAreasQuestion(callNumberDescriptionPairs[currentQuestionIndex]);
            }
            else
            {
                // All questions have been answered
                MessageBox.Show("Congratulations! You've completed all the questions.");
            }
        }

        private void DisplayIdentifyingAreasQuestion(CallNumberDescriptionPair pair)
        {
            // Clear the previous question and answers
            CallNumberListBox.Items.Clear();
            UserInputListBox.Items.Clear();

            // Randomly select four items for the first column
            List<string> randomItems = GetRandomItems(pair.Description, 4);

            // Create a list of possible answers
            List<string> possibleAnswers = new List<string> { pair.CallNumber };

            // Add three incorrect answers (randomly generated)
            for (int i = 0; i < 3; i++)
            {
                string incorrectAnswer = GetRandomCallNumber();
                while (possibleAnswers.Contains(incorrectAnswer))
                {
                    incorrectAnswer = GetRandomCallNumber();
                }
                possibleAnswers.Add(incorrectAnswer);
            }

            // Randomize the order of possible answers
            possibleAnswers = possibleAnswers.OrderBy(a => Guid.NewGuid()).ToList();

            // Display the question and possible answers
            CallNumberListBox.ItemsSource = randomItems;
            UserInputListBox.ItemsSource = possibleAnswers;
        }

        private List<string> GetRandomItems(string correctAnswer, int count)
        {
            List<string> items = new List<string> { correctAnswer };

            for (int i = 0; i < count - 1; i++)
            {
                string randomDescription = callNumberDescriptionPairs
                    .Where(pair => pair.Description != correctAnswer)
                    .OrderBy(x => Guid.NewGuid())
                    .Select(pair => pair.Description)
                    .First();

                items.Add(randomDescription);
            }

            // Shuffle the items
            items = items.OrderBy(a => Guid.NewGuid()).ToList();
            return items;
        }

        private string GetRandomCallNumber()
        {
            // Generate a random call number (replace with your logic)
            Random rand = new Random();
            return "XX" + rand.Next(1000, 9999);
        }

        private void UserInputListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserInputListBox.SelectedItem != null)
            {
                string selectedAnswer = UserInputListBox.SelectedItem.ToString();

                // Check if the selected answer is correct
                if (selectedAnswer == callNumberDescriptionPairs[currentQuestionIndex].CallNumber)
                {
                    MessageBox.Show("Correct! Well done.");
                }
                else
                {
                    MessageBox.Show("Incorrect. Try again.");
                }

                // Move to the next question
                currentQuestionIndex++;
                StartIdentifyingAreasTask();
            }
        }

        // ... Other methods ...

        private class CallNumberDescriptionPair
        {
            public string CallNumber { get; }
            public string Description { get; }

            public CallNumberDescriptionPair(string callNumber, string description)
            {
                CallNumber = callNumber;
                Description = description;
            }
        }
    }
}

