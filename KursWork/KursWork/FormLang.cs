using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace KursWork
{
    public partial class FormLang : Form
    {

        private SqlConnection conn = null;
        public FormLang()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LangDB"].ConnectionString);

            conn.Open();


            InitializeComponent();
            LoadUserData();
            ShowPanel(panelLangChoose);
        }

        //Блок кода для получения ID
        private int GetID(string username)
        {
            string query = "select id from [user] where username = @username";
            SqlCommand comd = new SqlCommand(query, conn);
            int userId = 0;

            comd.Parameters.AddWithValue("@username", username);

            SqlDataReader reader = comd.ExecuteReader();
            if (reader.Read())
            {
                userId = (int)reader["id"];
            }
            reader.Close();
            return userId;
         
        }

        //Блок кода для получения ID для английского
        private int GetWId(string word)
        {
            
            string query = "Select id from eng_words where word = @word";
            SqlCommand cmd = new SqlCommand(query, conn);
            int wordId = 0;

            cmd.Parameters.AddWithValue("@word", word);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                wordId = (int)reader["id"];
            }
            reader.Close();
            return wordId;
        }
        private int GetThId(string type)
        {
            string query = "Select id from eng_theory where type = @type";
            SqlCommand cmd = new SqlCommand(query, conn);
            int taskId = 0;
            cmd.Parameters.AddWithValue("@type", type);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                taskId = (int)reader["id"];
            }
            reader.Close();
            return taskId;
        }


        //Блок кода для получения ID для немецкого
        private int GetWIdGer(string word)
        {
            string query = "Select id from ger_words where word = @word";
            SqlCommand cmd = new SqlCommand(query, conn);
            int wordId = 0;

            cmd.Parameters.AddWithValue("@word", word);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                wordId = (int)reader["id"];
            }
            reader.Close();
            return wordId;
        }

        private int GetTheIdGer(string type)
        {
            string query = "Select id from ger_theory where type = @type";
            SqlCommand cmd = new SqlCommand(query, conn);
            int taskId = 0;
            cmd.Parameters.AddWithValue("@type", type);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                taskId = (int)reader["id"];
            }
            reader.Close();
            return taskId;
        }


        //Грузим данные и Form1 для того чтобы работать именно с тем пользователем который нам нужен________________________________________________________
        private int[] GetValuesFromDatabase()
        {
            int[] valuesFromDatabase = new int[3]; 

            
            string query = "SELECT [progress_eng_word], [progress_eng_pract], [progress_eng_theory] FROM [user] WHERE username=@username";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@username", Form1.LoggedInUsername);

            
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
               
                for (int i = 0; i < 3; i++)
                {
                    valuesFromDatabase[i] = reader.GetInt32(i);
                }
            }

            reader.Close(); // Закрываем reader после использования

            return valuesFromDatabase;
        }

        // Метод для установки значений прогресс-баров
        private void SetProgressBarValues(int[] values)
        {
            // Установка значений прогресс-баров
            // Предположим, что progressBarEngWords, progressBarEngPract и progressBarEngTheory - это ваши прогресс-бары на форме
            progressBarEngWords.Value = values[0];
            progressBarEngPract.Value = values[1];
            progressBarEngTheory.Value = values[2];
        }


        // Метод для обновления прогресс-баров с использованием значений из базы данных
        private void UpdateProgressBars()
        {
            // Получение значений из базы данных
            int[] valuesFromDatabase = GetValuesFromDatabase();

            // Установка значений прогресс-баров
            SetProgressBarValues(valuesFromDatabase);
        }


        //Блок для загрузки данных о прогрессе пользователя
        private void LoadUserData()
        {

            
            string username = Form1.LoggedInUsername;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Нет такого пользователя");
                return;
            }
           

            // Получаем данные пользователя из базы данных
            string query = "SELECT [username] FROM [user] WHERE [username]=@username";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        labelGreeting.Text = "Добро пожаловать, " + reader["username"].ToString();
                        // Вы можете загрузить и другие данные пользователя
                    }
                    reader.Close();
                }
                
            }
            

        }


        //Блок кода для загрузки слов и отметки слов____________________________________________________________________________________
        private void UpdateProgressEngWords(int userId)
        {
            // Подсчитать количество элементов с user_id = id в таблице user_learned_words
            string countQuery = "SELECT COUNT(*) FROM user_learned_words WHERE user_id = @userId";
            SqlCommand countCommand = new SqlCommand(countQuery, conn);
            countCommand.Parameters.AddWithValue("@userId", userId);

            int wordCount = (int)countCommand.ExecuteScalar();

            // Обновить progress_eng_words в таблице user
            string updateQuery = "UPDATE [user] SET progress_eng_word = @wordCount WHERE id = @userId";
            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@wordCount", wordCount);
            updateCommand.Parameters.AddWithValue("@userId", userId);

            updateCommand.ExecuteNonQuery();
        }
        private void MarkWordAsLearned(int userId, int wordId)
        {
            string query = "INSERT INTO user_learned_words (user_id, word_id) VALUES (@UserId, @WordId)";

            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);
            comm.Parameters.AddWithValue("@WordId", wordId);

            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            UpdateProgressEngWords(userId);
        }
        private void LoadWordAndTranslation()
        {
            //GetID(Form1.LoggedInUsername);
            string query = "SELECT id, word, translation FROM eng_words WHERE id NOT IN (SELECT word_id FROM user_learned_words WHERE user_id = @UserId) ORDER BY NEWID()"; // Замените на вашу таблицу и условия
            labelEngTranslation.Visible = false;


            SqlCommand comman = new SqlCommand(query, conn);
            comman.Parameters.AddWithValue("@UserId", GetID(Form1.LoggedInUsername));

            try
            {
                SqlDataReader reader = comman.ExecuteReader();

                if (reader.Read())
                {
                    string word = reader["word"].ToString();
                    string translation = reader["translation"].ToString();
                    int wordId = (int)reader["id"]; // Предполагается, что в вашей таблице есть поле word_id
                    

                    labelEngWord.Text = word;
                    labelEngTranslation.Text = translation;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            
        }


        //Методы для работы с теорией__________________________________________________________________________________________________
        private string GetTextByType(string type)
        {
            string query = "SELECT text FROM eng_theory WHERE type = @type";
            string result = null;
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@type", type);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                result = reader["text"].ToString();

            }
            reader.Close();


            //using (SqlDataReader reader = command.ExecuteReader())
            //{
            //    if (reader.Read())
            //    {
            //        result = reader["text"].ToString();
                    
            //    }
            //    reader.Close();

            //}

            return result;
        }

        private void UpdateProgressEngTheory(int userId)
        {
            // Подсчитать количество элементов с user_id = id в таблице user_learned_words
            string countQuery = "SELECT COUNT(*) FROM user_learned_theory WHERE user_id = @userId";
            SqlCommand countCommand = new SqlCommand(countQuery, conn);
            countCommand.Parameters.AddWithValue("@userId", userId);

            int wordCount = (int)countCommand.ExecuteScalar();

            // Обновить progress_eng_words в таблице user
            string updateQuery = "UPDATE [user] SET progress_eng_theory = @wordCount WHERE id = @userId";
            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@wordCount", wordCount);
            updateCommand.Parameters.AddWithValue("@userId", userId);

            updateCommand.ExecuteNonQuery();
        }
        private void MarkTheoryAsLearned(int userId, int taskId)// для кнопки "теперь я занаю"
        {
            string query = "INSERT INTO user_learned_theory (user_id, task_id) VALUES (@UserId, @TaskId)";

            


            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);
            comm.Parameters.AddWithValue("@TaskId", taskId);

            
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            UpdateProgressEngTheory(userId);
        }

        private bool GetElement(int userId)
        {
            bool element = false;

            string query = "SELECT id, type FROM eng_theory WHERE id IN (SELECT task_id FROM user_learned_theory WHERE user_id = @UserId) ORDER BY NEWID()"; // Замените на вашу таблицу и условия

            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);

            SqlDataReader reader = comm.ExecuteReader();


            if (reader.Read())
            {

            
                string type = reader["type"].ToString();


                if (type == comboBox1.Text)
                {
                    return element;
                }
                else
                    element = true;
                    
            }
            reader.Close();
            return element;
        }


        //Методы для работы с панелью практики_________________________________________________________________________________________






        //Методы для работы с немецким языком------------------------------------------------------------------------------------------
        //Получение данных
        private int[] GetValuesFromDatabaseGer()
        {
            int[] valuesFromDatabase = new int[3]; // Массив для хранения трех значений прогресса

            // Запрос к базе данных для получения значений
            string query = "SELECT [progress_ger_word], [progress_ger_pract], [progress_ger_theory] FROM [user] WHERE username=@username";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@username", Form1.LoggedInUsername);

            // Выполнение запроса и получение результата
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                // Получение значений из результата запроса и сохранение в массиве
                for (int i = 0; i < 3; i++)
                {
                    valuesFromDatabase[i] = reader.GetInt32(i);
                }
            }

            reader.Close(); // Закрываем reader после использования

            return valuesFromDatabase;
        }

        private void SetProgressBarValuesGer(int[] values)
        {
            // Установка значений прогресс-баров
            // Предположим, что progressBarEngWords, progressBarEngPract и progressBarEngTheory - это ваши прогресс-бары на форме
            progressBarGerWords.Value = values[0];
            progressBarGerPract.Value = values[1];
            progressBarGerTheory.Value = values[2];
        }

        private void UpdateProgressBarsGer()
        {
            // Получение значений из базы данных
            int[] valuesFromDatabase = GetValuesFromDatabaseGer();

            // Установка значений прогресс-баров
            SetProgressBarValuesGer(valuesFromDatabase);
        }

        //Рабта со словами
        private void UpdateProgressGerWords(int userId)
        {
            // Подсчитать количество элементов с user_id = id в таблице user_learned_words
            string countQuery = "SELECT COUNT(*) FROM user_learned_words_ger WHERE user_id = @userId";
            SqlCommand countCommand = new SqlCommand(countQuery, conn);
            countCommand.Parameters.AddWithValue("@userId", userId);

            int wordCount = (int)countCommand.ExecuteScalar();

            // Обновить progress_eng_words в таблице user
            string updateQuery = "UPDATE [user] SET progress_ger_word = @wordCount WHERE id = @userId";
            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@wordCount", wordCount);
            updateCommand.Parameters.AddWithValue("@userId", userId);

            updateCommand.ExecuteNonQuery();
        }
        private void MarkWordAsLearnedGer(int userId, int wordId)
        {
            string query = "INSERT INTO user_learned_words_ger (user_id, word_id) VALUES (@UserId, @WordId)";

            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);
            comm.Parameters.AddWithValue("@WordId", wordId);

            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            UpdateProgressGerWords(userId);
        }
        private void LoadWordAndTranslationGer()
        {
            //GetID(Form1.LoggedInUsername);
            string query = "SELECT id, word, translation FROM ger_words WHERE id NOT IN (SELECT word_id FROM user_learned_words_ger WHERE user_id = @UserId) ORDER BY NEWID()"; // Замените на вашу таблицу и условия
            labelTger.Visible = false;


            SqlCommand comman = new SqlCommand(query, conn);
            comman.Parameters.AddWithValue("@UserId", GetID(Form1.LoggedInUsername));

            try
            {
                SqlDataReader reader = comman.ExecuteReader();

                if (reader.Read())
                {
                    string word = reader["word"].ToString();
                    string translation = reader["translation"].ToString();
                    int wordId = (int)reader["id"]; // Предполагается, что в вашей таблице есть поле word_id


                    labelWger.Text = word;
                    labelTger.Text = translation;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }


        //Работа с теорией
        private string GetTextByTypeGer(string type)
        {
            string query = "SELECT text FROM ger_theory WHERE type = @type";
            string result = null;
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@type", type);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                result = reader["text"].ToString();

            }
            reader.Close();

            //using (SqlDataReader reader = command.ExecuteReader())
            //{
            //    if (reader.Read())
            //    {
            //        result = reader["text"].ToString();

            //    }
            //    reader.Close();

            //}

            return result;
        }

        private void UpdateProgressGerTheory(int userId)
        {
            // Подсчитать количество элементов с user_id = id в таблице user_learned_words
            string countQuery = "SELECT COUNT(*) FROM user_learned_theory_ger WHERE user_id = @userId";
            SqlCommand countCommand = new SqlCommand(countQuery, conn);
            countCommand.Parameters.AddWithValue("@userId", userId);

            int wordCount = (int)countCommand.ExecuteScalar();

            // Обновить progress_eng_words в таблице user
            string updateQuery = "UPDATE [user] SET progress_ger_theory = @wordCount WHERE id = @userId";
            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@wordCount", wordCount);
            updateCommand.Parameters.AddWithValue("@userId", userId);

            updateCommand.ExecuteNonQuery();
        }
        private void MarkTheoryAsLearnedGer(int userId, int taskId)
        {
            string query = "INSERT INTO user_learned_theory_ger (user_id, task_id) VALUES (@UserId, @TaskId)";




            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);
            comm.Parameters.AddWithValue("@TaskId", taskId);


            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            UpdateProgressGerTheory(userId);
        }

        private bool GetElementGer(int userId)
        {
            bool element = false;

            string query = "SELECT id, type FROM ger_theory WHERE id NOT IN (SELECT task_id FROM user_learned_theory_ger WHERE user_id = @UserId) ORDER BY NEWID()"; 

            SqlCommand comm = new SqlCommand(query, conn);
            comm.Parameters.AddWithValue("@UserId", userId);
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {


                string type = reader["type"].ToString();


                if (type == comboBox2.Text)
                {
                    return element;
                }
                else
                    element = true;

            }
            reader.Close();
            return element;
        }
        private void ShowPanel(Panel panel)
        {
            // Скрываем все панели
            panelLangChoose.Visible = false;
            panelStatsEng.Visible = false;
            panelEngWords.Visible = false;
            panelEngTheory.Visible = false;
            panelEngPract.Visible = false;
            

            //Панели немецкого языка
            panelStatsGer.Visible = false;
            panelGerWords.Visible = false;
            panelGrrTheory.Visible = false;

            //Обнавляем при переходе на статитстику
            if (panel == panelStatsEng)
                UpdateProgressBars();

            if (panel == panelStatsGer)
                UpdateProgressBarsGer();



            // Показываем указанную панель
            panel.Visible = true;

           

            
        }
       
        
        //Область кнопок для переключения
        private void buttonEngChange_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsEng);  // Переход на вторую панель
        }

        private void buttonWords_Click(object sender, EventArgs e)
        {
            LoadWordAndTranslation();
            ShowPanel(panelEngWords);  // Переход на вторую панель
            
        }
        private void buttonEngTheory_Click(object sender, EventArgs e)
        {
            ShowPanel(panelEngTheory);

        }
        private void buttonTheoryBack_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsEng);
        }


        //Блок кнопок для английских слов
        private void buttonEngWordBack_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsEng);
        }
        private void buttonShow_Click(object sender, EventArgs e)
        {
            labelEngTranslation.Visible = true;
        }
        private void buttonKnoladge_Click(object sender, EventArgs e)
        {
            MarkWordAsLearned(GetID(Form1.LoggedInUsername), GetWId(labelEngWord.Text));
            LoadWordAndTranslation();

        }
        // Кнопки для изучения теории
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = comboBox1.SelectedItem.ToString();
            string textValue = GetTextByType(selectedType);

            switch (selectedType)
            {
                case "Структура простого предложения":
                    textBox1.Text = textValue;
                    break;
                case "Настоящее время":
                    textBox1.Text = textValue;
                    break;
            }
        }
        private void buttonKnowTheory_Click(object sender, EventArgs e)
        {
            if (GetElement(GetID(Form1.LoggedInUsername)))
            {
                MarkTheoryAsLearned(GetID(Form1.LoggedInUsername), GetThId(comboBox1.Text));
            }
            else
            {
                MessageBox.Show("Вы уже знаете эту теорию");
            }

        }




        //Блок кнопок для практики




        //Блок кнопок для немецкого------------------------------------------------------------------------------------------
        //Изменение экранов
        private void buttonEngChangeGer_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsGer);
        }

        private void buttonBackGer_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsGer);
        }

        private void buttonWordsGer_Click(object sender, EventArgs e)
        {
            LoadWordAndTranslationGer();
            ShowPanel(panelGerWords);
        }

        private void buttonGerTheory_Click(object sender, EventArgs e)
        {
            ShowPanel(panelGrrTheory);
        }

        private void buttonGerTheoryBack_Click(object sender, EventArgs e)
        {
            ShowPanel(panelStatsGer);
        }
        //Кнопки для слов
        private void buttonGerShow_Click(object sender, EventArgs e)
        {
            labelTger.Visible = true;
        }

        private void buttonGerWordKnow_Click(object sender, EventArgs e)
        {
            MarkWordAsLearnedGer(GetID(Form1.LoggedInUsername), GetWIdGer(labelWger.Text));
            LoadWordAndTranslationGer();
        }



        //Кнопки для теории

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = comboBox2.SelectedItem.ToString();
            string textValue = GetTextByTypeGer(selectedType);

            switch (selectedType)
            {
                case "Структура простого предложения":
                    textBox2.Text = textValue;
                    break;
                case "Настоящее время":
                    textBox2.Text = textValue;
                    break;
            }
        }


        private void buttonGerKnow_Click(object sender, EventArgs e)
        {
            if (GetElementGer(GetID(Form1.LoggedInUsername)))
            {
                MarkTheoryAsLearnedGer(GetID(Form1.LoggedInUsername), GetTheIdGer(comboBox2.Text));
            }
            else
            {
                MessageBox.Show("Вы уже знаете эту теорию");
            }
        }

      
    }


}
