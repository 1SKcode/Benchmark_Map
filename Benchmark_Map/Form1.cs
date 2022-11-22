using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;


namespace Benchmark_Map
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region ТЕСТЫ
        public double Test1() // Метод выполнения теста 1 (сортировка массива). Возвращается время выполенения метода В СЕКУНДАХ+миллисекунды
        {
            int[] array = new int[45000];
            Random random = new Random();

            for (int i = 0; i < array.Length; i++)  // Инициализация массива рандомными значениями 
            {
                array[i] = random.Next(20001); // Присваиваем i элементу рандомное значение от 0 до 20000
            }

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start(); // Начинаем отсчет затраченного времени на интервале (остановка на 50 строке)
            for (int i = 0; i < array.Length; i++) // Начало выполнения сортировки пузырьком
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            stopWatch.Stop(); // Останавливаем отсчет затраченного времени на интервале (запуск был на 37 строке)

            return stopWatch.ElapsedMilliseconds / 1000f; // Возвращаем значение затраченных секунд на выполнение теста 1
        }

        public double Test2() // Метод выполнения теста 2 (графические отрисовки). Возвращается время выполенения метода В СЕКУНДАХ+миллисекунды
        {
            Graphics g;
            g = panel1.CreateGraphics();
            float x = 0f;

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start(); // Начинаем отсчет затраченного времени на интервале (остановка на 76 строке)
            for (int i = 0; i < 3000; i++) // Отрисовываем 3000 раз 8 фигур (всего 24000)
            {
                g.FillEllipse(Brushes.Black, x, 200, 10, 10);
                g.FillRectangle(Brushes.Red, x, 210, 10, 10);
                g.FillEllipse(Brushes.Black, x, 220, 10, 10);
                g.FillRectangle(Brushes.Red, x, 230, 10, 10);
                g.FillEllipse(Brushes.Black, x, 240, 10, 10);
                g.FillRectangle(Brushes.Red, x, 250, 10, 10);
                g.FillEllipse(Brushes.Black, x, 260, 10, 10);
                g.FillRectangle(Brushes.Red, x, 270, 10, 10);
                x = x + 0.09f; // Координату Х в конце каждой итерации инкрементируем на небольшое значение,
                               // тем самым по оси Х происходит небольшое смещение
            }
            stopWatch.Stop(); // Останавливаем отсчет затраченного времени на интервале (запуск был на 62 строке)
            g.Clear(SystemColors.ActiveCaption);

            return stopWatch.ElapsedMilliseconds / 1000f; // Возвращаем значение затраченных секунд на выполнение теста 1
        }

        public double Test3() // Метод выполнения теста 3 (Запись и чтение файла). Возвращается время выполенения метода В СЕКУНДАХ+миллисекунды
        {
            Stopwatch stopWatch = new Stopwatch();
            string info = "ЭТА СТРОКА ЗАПИШЕТСЯ В ФАЙЛ TEST_3.txt 50000 РАЗ. ВРЕМЯ ЗАПИСИ ФАЙЛА БУДЕТ ЗАМЕРЯТЬСЯ";

            stopWatch.Start(); // Начинаем отсчет затраченного времени на интервале (остановка на 91 строке)
            for (int i = 0; i < 50000; i++)
            {
                File.AppendAllText("TEST_3.txt", info); // Метод создает или открывает файл,
                                                        // записывает в него указанную строку по указанному пути и закрывается
            }
            stopWatch.Stop(); // Останавливаем отсчет затраченного времени на интервале (запуск был на 86 строке)

            File.Delete("TEST_3.txt"); // Удаление только что заполненного текстового файла

            return stopWatch.ElapsedMilliseconds / 1000f; // Возвращаем значение затраченных секунд на выполнение теста 1
        }
        #endregion

        #region КЛИКИ НА КНОПКИ

        private async void button1_Click(object sender, EventArgs e) // Событие нажатия на кнопку "НАЧАТЬ ТЕСТЫ"
        {
            string namePC = textBox1.Text;

            if (textBox1.Text.Length > 0)
            {
                button1.Visible = false; // Делаем невидимой кнопку "НАЧАТЬ ТЕСТЫ"
                button2.Visible = false; // Делаем невидимой кнопку "Удалить записи"
                label3.Text = " "; // Очищаем лог для последующих записей в него

                double test1Time = 0;
                double test2Time = 0;
                double test3Time = 0;

                await Task.Run(() => // Асинхронные запуски методов и изменение свойств (асинхронность нужна для того, чтобы окно не зависло)
                {
                    label3.Text += "\n Начало сортировки \n......";
                    test1Time = Test1(); // Выполнение теста 1
                    label3.Text += "\n Сортировка завершена \n ...ТЕСТ 1 ЗАВЕРШЕН...";

                    Thread.Sleep(2000); // Приостанавливаем поток на 2 секунды (смена теста)

                    label3.Text += "\n Начало отрисовки графики... \n ......";
                    test2Time = Test2(); // Тест 2
                    label3.Text += "\n Отрисовка графики завершена \n ...ТЕСТ 2 ЗАВЕРШЕН...";

                    Thread.Sleep(2000); // Приостанавливаем поток на 2 секунды (смена теста)

                    label3.Text += "\n Начало записи в файл \n......";
                    test3Time = Test3(); // Тест 3
                    label3.Text += "\n Запись в файл завершена \n ...ТЕСТ 3 ЗАВЕРШЕН...";

                });
                label3.Text += "\n ТЕСТ 1 - " + test1Time + " секунд";
                label3.Text += "\n ТЕСТ 2 - " + test2Time + " секунд";
                label3.Text += "\n ТЕСТ 3 - " + test3Time + " секунд";

                button1.Visible = true;
                button2.Visible = true;

                dataGridView1.Rows.Add(namePC, test1Time, test2Time, test3Time, Math.Pow(test1Time * test2Time * test3Time, 1.0 / 3));
                dataGridView2.Rows.Add(
                    namePC,
                    test1Time / Convert.ToDouble(dataGridView1[1, 0].Value),
                    test2Time / Convert.ToDouble(dataGridView1[2, 0].Value),
                    test3Time / Convert.ToDouble(dataGridView1[3, 0].Value),
                    Math.Pow((test1Time / Convert.ToDouble(dataGridView1[1, 0].Value)) *
                            (test2Time / Convert.ToDouble(dataGridView1[2, 0].Value)) *
                            (test3Time / Convert.ToDouble(dataGridView1[3, 0].Value)), 1.0 / 3)
                    );


            }
            else MessageBox.Show("Введите имя тестируемого ПК", "", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e) // Собитие при нажатии на кнопку "Удалить записи"
        {
            File.WriteAllText("sample.txt", string.Empty); // Перезаписываем файл (в итоге имеем пустой файл txt)
            dataGridView1.Rows.Clear(); // Очищаем и сбрасываем таблицу
            dataGridView1.Refresh();

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
        }
        #endregion

        #region ЗАГРУЗКА ТАБЛИЦЫ ПРИ ОТКРЫТИТИ И СОХРАНЕНИЕ ПРИ ЗАКРЫТИТИ
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // Событие закрытия окна, тут выполняется запись предыдущих тестов
        {

            StreamWriter file = new StreamWriter("sample.txt");
            string sLine = "";

            // Этот цикл for повторяется через каждую строку в таблице
            for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
            {
                //Это для цикла, проходящего через каждый столбец, и номер строки
                //передается из цикла for выше.
                for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                {
                    sLine = sLine + dataGridView1.Rows[r].Cells[c].Value;
                    if (c != dataGridView1.Columns.Count - 1)
                    {
                        //Символ "|" добавляется в качестве разделителя значений в таблице
                        sLine = sLine + "|";
                    }
                }
                //Текст записывается в текстовый файл построчно
                file.WriteLine(sLine);
                sLine = "";
            }

            file.Close();
        }

        private void Form1_Load(object sender, EventArgs e) // Событие загрузки окна, тут выполняется чтение записей из текстового файла в нашу таблицу в программе
        {
            string[] lines = File.ReadAllLines("sample.txt");
            string[] values;

            // Этот цикл for повторяется через каждую строку в таблице
            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|'); // Разделяем значения через поиск символа "|" и присваиваем значение в i элемент массива
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                dataGridView1.Rows.Add(row);
            }

            for (int i = 0; i < dataGridView1.RowCount; i++) // Запись во вторую таблицу осуществляется через расчеты. Т.к нужно всего лишь посчитать коэффициенты 
            {
                dataGridView2.Rows.Add(
                    dataGridView1[0, i].Value,
                    Convert.ToDouble(dataGridView1[1, i].Value) / Convert.ToDouble(dataGridView1[1, 0].Value),
                    Convert.ToDouble(dataGridView1[2, i].Value) / Convert.ToDouble(dataGridView1[2, 0].Value),
                    Convert.ToDouble(dataGridView1[3, i].Value) / Convert.ToDouble(dataGridView1[3, 0].Value),
                    Convert.ToDouble(dataGridView1[4, i].Value) / Convert.ToDouble(dataGridView1[4, 0].Value)
                    );
            }
        }
        #endregion

    }
}
