using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace laba1
{
    public partial class Form1 : Form
    {
        private string currentFilePath = null;
        private ToolStripLabel dateLabel;
        private ToolStripLabel timeLabel;
        private ToolStripLabel layoutLabel;
        
        private void UpdateStatusLabels(object sender, EventArgs e)
        {

            dateLabel.Text = "Дата" + " " + DateTime.Now.ToLongDateString();
            timeLabel.Text = "Время" + " " + DateTime.Now.ToLongTimeString();


            var currentLayout = InputLanguage.CurrentInputLanguage.LayoutName;
            layoutLabel.Text = "Раскладка: " + currentLayout;
        }
        public void Back() 
        {
            FastColoredTextBox tb = inputTextBox as FastColoredTextBox;
            if (tb.UndoEnabled)
                tb.Undo();
        }
        public void Next() 
        {
            FastColoredTextBox tb = inputTextBox as FastColoredTextBox;
            if (tb.RedoEnabled)
                tb.Redo();
        }
        public void In() 
        { 
            inputTextBox.Paste();
        }
        public void Copy()
        {
            if (inputTextBox.SelectionLength > 0)
            {
                inputTextBox.Copy();}
        }
        public void Cut() 
        { 
            if (inputTextBox.SelectionLength > 0)
            { 
                inputTextBox.Cut();
            }
        }
        public void Help() 
        { 
            MessageBox.Show("Описание функций меню\r\n\r\nФайл - производит действия с файлами\r\n\r\nСоздать - создает файл\r\nОткрыть - открывает файл\r\nСохранить - сохраняет изменения в файле\r\nСохранить как - сохраняет изменения в новый файл\r\nВыход - осуществляет выход из программы\r\n\r\nПравка - осуществляет изменения в файле\r\n\r\nОтменить - отменяет последнее изменение\r\nПовторить - повторяет последнее действие\r\nВырезать - вырезает выделенный фрагмент\r\nКопировать - копирует выделенный фрагмент\r\nВставить - вставляет выделенный фрагмент\r\nУдалить - удаляет выделенный фрагмент\r\nВыделить все - выделяет весь текст\r\n\r\nСправка - показывает справочную информацию\r\n\r\nВызов справки - описывает функции меню\r\nО программе - содержит информацию о программе", "Справка");
        }
        public void About() 
        { 
            MessageBox.Show("Данная программа это лабораторная работа номер 1 по предмету Теория формальных языков и компиляторов выполнил работу Студент группы АВТ-114 Белетков Андрей", "О программе");
        }
        public void Create()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, "");
                currentFilePath = saveFileDialog.FileName;
            }
        }
        public void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                currentFilePath = openFileDialog.FileName;
            }
        }
        public void Save()
        {
            if (currentFilePath != null)
            {
                File.WriteAllText(currentFilePath, inputTextBox.Text);

            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, inputTextBox.Text);
                    currentFilePath = saveFileDialog.FileName;
                }
            }
        }

        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in filePaths)
            {

                string fileContent = File.ReadAllText(filePath);

                inputTextBox.AppendText(fileContent + Environment.NewLine);
            }
        }

        public Form1() 
        { 
            InitializeComponent();

            this.AllowDrop = true;
            this.DragEnter += Form_DragEnter;
            this.DragDrop += Form_DragDrop;

            dateLabel = new ToolStripLabel();
            dateLabel.Text = "";
            timeLabel = new ToolStripLabel();
            timeLabel.Text = "";
            layoutLabel = new ToolStripLabel();


            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);
            statusStrip1.Items.Add(layoutLabel);


            var timer = new Timer { Interval = 1000 };
            timer.Tick += UpdateStatusLabels;
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.Windows.Forms.ToolTip t1 = new System.Windows.Forms.ToolTip();
            t1.SetToolTip(buttonCreate, "Создать");

            System.Windows.Forms.ToolTip t2 = new System.Windows.Forms.ToolTip();
            t2.SetToolTip(buttonOpen, "Открыть");

            System.Windows.Forms.ToolTip t3 = new System.Windows.Forms.ToolTip();
            t3.SetToolTip(buttonSave, "Сохранить");

            System.Windows.Forms.ToolTip t4 = new System.Windows.Forms.ToolTip();
            t4.SetToolTip(buttonCopy, "Копировать");

            System.Windows.Forms.ToolTip t5 = new System.Windows.Forms.ToolTip();
            t5.SetToolTip(buttonCut, "Вырезать");

            System.Windows.Forms.ToolTip t6 = new System.Windows.Forms.ToolTip();
            t6.SetToolTip(buttonIn, "Вставить");

            System.Windows.Forms.ToolTip t7 = new System.Windows.Forms.ToolTip();
            t7.SetToolTip(buttonBack, "Отменить");

            System.Windows.Forms.ToolTip t8 = new System.Windows.Forms.ToolTip();
            t8.SetToolTip(buttonNext, "Повторить");

            System.Windows.Forms.ToolTip t9 = new System.Windows.Forms.ToolTip();
            t9.SetToolTip(buttonInfo, "О программе");

            System.Windows.Forms.ToolTip t10 = new System.Windows.Forms.ToolTip();
            t10.SetToolTip(buttonHelp, "Вызов справки");

            System.Windows.Forms.ToolTip t11 = new System.Windows.Forms.ToolTip();
            t11.SetToolTip(buttonPlay, "Пуск");
        }      


        private void buttonHelp_Click(object sender, EventArgs e) 
        { 
            Help();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Help();
        }

        private void buttonInfo_Click(object sender, EventArgs e) 
        { 
            About();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            About();
        }

        private void buttonCopy_Click(object sender, EventArgs e) 
        {
            Copy();
        }

        private void buttonCut_Click(object sender, EventArgs e) 
        { 
            Cut();
        }

        private void buttonIn_Click(object sender, EventArgs e) 
        { 
            In();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Copy();
        }

        private void inToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            In();
        }

        private void buttonBack_Click(object sender, EventArgs e) 
        { 
            Back();
        }

        private void buttonNext_Click(object sender, EventArgs e) {
            
            Next(); 
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Back();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            Next();
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputTextBox.SelectedText != "")
            {
                inputTextBox.Text = inputTextBox.Text.Remove(inputTextBox.SelectionStart, inputTextBox.SelectionLength);
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            inputTextBox.SelectAll();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Сохранить изменения перед выходом?", "Подтверждение", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                Save();
            }
            else if (result == DialogResult.Cancel)
            {
                
                return;
            }

            Application.Exit();

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Open();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, inputTextBox.Text);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Save();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e) 
        { 
            Create();
        }

        private void buttonCreate_Click(object sender, EventArgs e) 
        { 
            Create();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        { 
            Open();
        }

        private void buttonSave_Click(object sender, EventArgs e) 
        { 
            Save();
        }


        private void inputTextBox_Load(object sender, EventArgs e)
        {
        }

        
        public class Parser
        {
            public string str = "";
            static Lexeme _lexeme = null;
            private List<Lexeme> lexemes;
            private int position;
            public int counter;
            public string right_str;
            public int flag;
            public string result;

            public Parser(List<Lexeme> lexemes)
            {
                this.lexemes = lexemes;
                this.position = 0;
                this.counter = 0;
                flag = lexemes.Count;
            }


            public void parser(DataGridView dataGridView1)
            {
                Scan();
               DEF(dataGridView1);

            }
            void DEF(DataGridView dataGridView1)
            {
                try
                {
                    S(dataGridView1);
                    if (position == lexemes.Count)
                    {
                        dataGridView1.Rows.Add("Парсинг прошёл успешно ");
                        counter = 1;
                    }
                    else
                    {
                        dataGridView1.Rows.Add("Парсинг прошёл не успешно ");
                        counter = 0;
                    }
                }
                catch (Exception ex)
                {

                    dataGridView1.Rows.Add("Ошибка: " + ex.Message);
                }
            }
            void S(DataGridView dataGridView1)
            {
                result += "S ";
                if (_lexeme != null &&  _lexeme.Type == LexemeType.Select)
                {

                    result += "select ";
                    Scan();
                    X(dataGridView1);

                }
                else 
                    Error();
                if (_lexeme != null && _lexeme.Type == LexemeType.From)
                {
                    result += "from ";
                    Scan();
                    X(dataGridView1);
                }
                else
                    Error();
            }

            void X(DataGridView dataGridView1)
            {
                result += "X ";


                    if (_lexeme != null && _lexeme.Type == LexemeType.Word)
                    {
                        A(dataGridView1);
                        Y(dataGridView1);
                    }
                    else if (_lexeme != null && _lexeme.Type != LexemeType.Word)
                    {
                        Error();
                    }
                    else if (_lexeme == null)
                    {
                        result += "";
                    }




            }
            void Y(DataGridView dataGridView1)
            {
                result += "Y ";

                if (_lexeme != null && _lexeme.Type == LexemeType.Comma)
                {

                    result += ", ";
                    Scan();
                    A(dataGridView1);
                    Y(dataGridView1);
                }
                else if (_lexeme == null || _lexeme.Type == LexemeType.From)
                {
                    result += "e ";
                }
                else if (_lexeme != null && _lexeme.Type != LexemeType.Comma) 
                { 
                    Error();
                }
                



            }
            void A(DataGridView dataGridView1)
            {
                result += "A ";
                if (_lexeme != null && _lexeme.Type == LexemeType.Word)
                {

                    result += "Word ";
                    Scan();
                }
                else if (_lexeme != null && _lexeme.Type != LexemeType.Word)
                {
                    Error();
                }
                else if (_lexeme == null)
                {
                    result += "";
                }

            }
            void Scan()
            { 
                if (position < lexemes.Count)
                {
                    // Пропускаем пробелы
                    while (position < lexemes.Count && lexemes[position].Type == LexemeType.Delimiter)
                    {
                        position++;
                    }

                    if (position < lexemes.Count)
                    {
                        _lexeme = lexemes[position];
                        position++;
                    }
                    else
                    {
                        _lexeme = null;
                        return;// Конец входного выражения
                    }
                }
                else
                {
                    _lexeme = null;
                    return;// Конец входного выражения
                }
            }

            static void Error()
            {
                Console.WriteLine("Ошибка синтаксиса");
            }






        }
        public class Lexeme
        {
            public int Code 
            { 
                get;
                set;
            }
            public LexemeType Type { get; set; }
            public string Token { get; set; }
            public int StartPosition { get; set; }
            public int EndPosition { get; set; }

            public Lexeme(int code, LexemeType type, string input, int startPosition, int endPosition)
            {
                Code = code;
                Type = type;
                Token = input.Substring(startPosition, endPosition - startPosition + 1);
                StartPosition = startPosition;
                EndPosition = endPosition;
            }
        }

        public enum LexemeType
        { 
            Comma,
            Select,
            Delimiter,
            From,
            Invalid,
            EndStr,
            NewStr,
            Word

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
        }

        private void buttonPlay_Click_1(object sender, EventArgs e)
        {

            parser();

        }
        private void parser() 
        {
            string input = inputTextBox.Text;

            Dictionary<LexemeType, int> lexemeCodes = new Dictionary<LexemeType, int>()
{


    { LexemeType.Comma, 1 },
    { LexemeType.Invalid, 2 },
    { LexemeType.Delimiter, 8 },
    { LexemeType.EndStr, 3 },
    { LexemeType.NewStr, 4 },
    { LexemeType.Select, 5 },
    { LexemeType.From, 7 },
    { LexemeType.Word, 6 },

};
            string[] select = { "select" };
            string[] from = { "from" };
            string[] comma = { "," };
            string[] delimiters = { " " };
            char[] endstring = { '\r' };
            char[] startstring = { '\n' };


            List<Lexeme> lexemes = new List<Lexeme>();

            int position = 0;
            while (position < input.Length)
            {
                bool found = false;

                foreach (string select_ in select)
                {
                    if (input.Substring(position).StartsWith(select_))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Select], LexemeType.Select, input, position, position + select.Length - 1));
                        position += select_.Length;
                        found = true;
                        break;
                    }
                }
                

                if (found) continue;
                foreach (string from_ in from)
                {
                    if (input.Substring(position).StartsWith(from_))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.From], LexemeType.From, input, position, position + from.Length - 1));
                        position += from_.Length;
                        found = true;
                        break;
                    }
                }
                if (found) continue;

                //_
                foreach (string delimiter in delimiters)
                {
                    if (input.Substring(position).StartsWith(delimiter))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Delimiter], LexemeType.Delimiter, input, position, position + delimiter.Length - 1));
                        position += delimiter.Length;
                        found = true;
                        break;
                    }
                }
                if (found) continue;
                // ","
                foreach (string comma_ in comma)
                {
                    if (input.Substring(position).StartsWith(comma_))
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Comma], LexemeType.Comma, input, position, position + comma_.Length - 1));
                        position += comma_.Length;
                        found = true;
                        break;
                    }
                }

                if (found) continue;
                foreach (char endstr in endstring)
                {
                    if (input[position] == endstr)
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.EndStr], LexemeType.EndStr, input, position, position));
                        position++;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //  \n
                foreach (char newstr in startstring)
                {
                    if (input[position] == newstr)
                    {
                        lexemes.Add(new Lexeme(lexemeCodes[LexemeType.NewStr], LexemeType.NewStr, input, position, position));
                        position++;
                        found = true;
                        break;
                    }
                }

                if (found) continue;

                //name
                if (char.IsLetter(input[position]))
                {
                    int start = position;
                    while (position < input.Length && char.IsLetterOrDigit(input[position]))
                    {
                        position++;
                    }
                    string Word = input.Substring(start, position - start);
                    lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Word], LexemeType.Word, input, start, position - 1));
                }
                //error
                else
                {
                    string invalid = input[position].ToString();
                    lexemes.Add(new Lexeme(lexemeCodes[LexemeType.Invalid], LexemeType.Invalid, input, position, position));
                    position++;
                }
            }

            dataGridView2.Rows.Clear();

            Parser parser = new Parser(lexemes);

            parser.parser(dataGridView1);
            string a = "Последовательность  вызова процедур обработки \n" + parser.result;
            MessageBox.Show(a);


            foreach (Lexeme lexeme in lexemes)
            {
                dataGridView1.Rows.Add(lexeme.Code, lexeme.Type, lexeme.Token, lexeme.StartPosition, lexeme.EndPosition);
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parser();
        }

        private void постановкаЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Тема работы: Объявление целочисленной константы с инициализацией на языке C/C++\r\n\r\n" +
                "Особенности языка: \r\n" +
                "Константы – это элементы данных, значения которых известны и в процессе выполнения программы не изменяются.\r\n" +
                "Для описания констант в языке C/C++ используется служебное слово const или constexpr.\r\n" +
                "Формат записи: const/constexpr тип_данных название_переменной=значение;.\r\n\r\n " +
                "Примеры верных строк из языка:\r\n " +
                "1. const int abc = 123; \r\n" +
                " 2. constexpr int b= 123; \r\n" +
                "3. const int d = -123;", "Постановка задачи");

        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработка грамматики\r\nОпределим грамматику целочисленных констант языка С/С++ G[‹Def›] в нотации Хомского с продукциями P:\r\n" +
                "1) DEF -> [‘const’|‘constexpr’] CONST \r\n" +
                "2) CONST -> ‘_’ INT\r\n" +
                "3) INT -> ‘int’ INTREM\r\n" +
                "4) INTREM -> ‘_’ ID\r\n" +
                "5) ID ->letter IDREM\r\n" +
                "6) IDREM -> letter IDREM\r\n" +
                "7) IDREM -> ‘=’EQUAL\r\n" +
                "8) EQUAL -> [‘+’ | ‘-’] NUMBER\r\n" +
                "9) NUMBER -> digit NUMBERREM\r\n" +
                "10) NUMBERREM -> digit NUMBERREM\r\n" +
                "11) NUMBERREM -> ;\r\n•\t" +
                "‹Digit› → “0” | “1” | “2” | “3” | “4” | “5” | “6” | “7” | “8” | “9”\r\n•\t" +
                "‹Letter› → “a” | “b” | “c” | ... | “z” | “A” | “B” | “C” | ... | “Z”\r\n" +
                "Следуя введенному формальному определению грамматики, представим G[‹Def›] ее составляющими:\r\n•\t" +
                "Z = ‹Def›;\r\n•\tVT = {a, b, c, ..., z, A, B, C, ..., Z, _, =, +, -, ;, ., 0, 1, 2, ..., 9};\r\n•\t" +
                "VN = {DEF, CONST, ID, IDREM, TYPE, EQUAL, NUM, NUMBER, NUMBERREM}.\r\n", "Грамматика");
        }
    

        private void методАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Граф автоматной грамматики", "Метод анализа");
        }

        private void диагностикаИНейтрализацияОшибокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("По методу Айронса", "Диагностика и нейтрализация ошибок");
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Тестовые_примеры.html");
        }

        private void списокЛитературыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Шорников Ю.В. Теория и практика языковых процессоров : учеб. пособие / Ю.В. Шорников. – Новосибирск: Изд-во НГТУ, 2004.\r\n" +
                "2. Gries D. Designing Compilers for Digital Computers. New York, Jhon Wiley, 1971. 493 p.\r\n" +
                "3. Теория формальных языков и компиляторов [Электронный ресурс] / Электрон. дан. URL: https://dispace.edu.nstu.ru/didesk/course/show/8594, свободный. Яз.рус. (дата обращения 01.04.2021).\r\n", "Список литературы");
        }

        private void исходныйКодПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "github.com/AndreyNSTU1/laba_-/tree/%D0%9A%D1%83%D1%80%D1%81%D0%BE%D0%B2%D0%B0%D1%8F";
            Process.Start("https://" + url);

        }

        private void классификацияГрамматикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Согласно классификации Хомского, грамматика G[‹Def›] является автоматной.\r\n" +
                "Правила (1)-(11) относятся к классу праворекурсивных продукций (A → aB | a | ε):\r\n" +
                 "1) DEF -> [‘const’|‘constexpr’] CONST \r\n" +
                "2) CONST -> ‘_’ INT\r\n" +
                "3) INT -> ‘int’ INTREM\r\n" +
                "4) INTREM -> ‘_’ ID\r\n" +
                "5) ID ->letter IDREM\r\n" +
                "6) IDREM -> letter IDREM\r\n" +
                "7) IDREM -> ‘=’EQUAL\r\n" +
                "8) EQUAL -> [‘+’ | ‘-’] NUMBER\r\n" +
                "9) NUMBER -> digit NUMBERREM\r\n" +
                "10) NUMBERREM -> digit NUMBERREM\r\n" +
                "11) NUMBERREM -> ;\r\n•\t");
        }
    }
}
