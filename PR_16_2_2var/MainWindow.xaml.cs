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

using Microsoft.Win32;

namespace PR_16_2_2var
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SubjectGrade> subjectsList;

        public MainWindow()
        {
            InitializeComponent();
            subjectsList = new List<SubjectGrade>();
            dgSubjects.ItemsSource = subjectsList;
        }

        // Добавление предмета
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtsubject.Text))
            {
                MessageBox.Show("Введи название предмета");
                return;
            }
            if (txtsubject.Text.All(char.IsDigit))
            {
                MessageBox.Show("Нельзя ввести предмет цифрами! Печатай снова!!!");
                return;
            }

            if (!int.TryParse(txtgrade.Text, out int grade) || grade < 1 || grade > 5)
            {
                MessageBox.Show("Оценка должна быть числом от 1 до 5");
                return;
            }

            subjectsList.Add(new SubjectGrade
            {
                Предмет = txtsubject.Text,
                Оценки = grade
            });
            RefreshDataGrid();

            txtsubject.Text = "";
            txtgrade.Text = "";
        }

        // Сохранение в CSV
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtspec.Text))
            {
                MessageBox.Show("Введи специальность", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtspec.Text.All(char.IsDigit))
            {
                MessageBox.Show("Нельзя ввести специальность цифрами! Печатай снова!!!");
                return;
            }

            if (subjectsList.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV файл (*.csv)|*.csv";
            saveDialog.DefaultExt = ".csv";

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    CsvService.SaveToCsv(saveDialog.FileName, txtspec.Text, subjectsList);
                    MessageBox.Show("Сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Загрузка из CSV
        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "CSV файл (*.csv)|*.csv";

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    var (speciality, loadedSubjects) = CsvService.LoadFromCsv(openDialog.FileName);

                    txtspec.Text = speciality;
                    subjectsList = loadedSubjects;
                    RefreshDataGrid();

                    MessageBox.Show($"Загружено {subjectsList.Count} предметов");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки: " + ex.Message);
                }
            }
        }

        // Обновление DataGrid
        private void RefreshDataGrid()
        {
            dgSubjects.ItemsSource = null;
            dgSubjects.ItemsSource = subjectsList;
        }
    }
}


