using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class TopicQuery
    {
        public bool UpdateTopicById(
            string nameTopic,
            int courseId,
            string? description,
            string videoTopic,
            string? audioTopic,
            string? documentTopic,
            string status,
            int id
        )
        {
            bool checkingUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Topics] SET [NameTopic] = @nameTopic, [CourseId] = @Courses, [Description] = @description, [Video] = @video, [Audio] = @audio, [DocumentTopic] = @documentTopic, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@nameTopic", nameTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Courses", courseId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@video", videoTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@audio", audioTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@documentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkingUpdate = true;
            }
            return checkingUpdate;
        }


        public bool DeleteItemTopic(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] =  @deletedAt WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                statusDelete = true;
                connection.Close();
            }
            //false: ko xoa dc --- true : xoa thanh cong
            return statusDelete;
        }


        public TopicDetail GetDetailTopicById(int id = 0)
        {
            TopicDetail topic = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topic.Id = Convert.ToInt32(reader["Id"]);
                        topic.NameTopic = reader["NameTopic"].ToString();
                        topic.CourseId = Convert.ToInt32(reader["CourseId"]);
                        topic.Description = reader["Description"].ToString();
                        topic.NameVideo = reader["Video"].ToString();
                        topic.NameAudio = reader["Audio"].ToString();
                        topic.NameDocumentTopic = reader["DocumentTopic"].ToString();
                        topic.viewCourseName = reader["CourseId"].ToString();
                        topic.Status = reader["Status"].ToString();
                    }
                    connection.Close();
                }
                
            }
            return topic;
        }


        public List<TopicDetail> GetAllDataTopics(string? keyword, string? filter)
        {
            string search = "%" + keyword + "%";
            List<TopicDetail> topics = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                if (filter != null)
                {
                    sqlQuery = "SELECT [to].*, [co].[NameCourse] FROM [Topics] AS [to] INNER JOIN [Courses] AS [co] ON [to].[CourseId] = [co].[Id] WHERE [to].[NameTopic] LIKE @keyword AND [to].[DeletedAt] IS NULL AND [to].[Status] = @status";
                }
                else
                {
                    sqlQuery = "SELECT [to].*, [co].[NameCourse] FROM [Topics] AS [to] INNER JOIN [Courses] AS [co] ON [to].[CourseId] = [co].[Id] WHERE [to].[NameTopic] LIKE @keyword AND [to].[DeletedAt] IS NULL";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@keyword", search ?? DBNull.Value.ToString());
                if (filter != null)
                {
                    cmd.Parameters.AddWithValue("@status", filter ?? DBNull.Value.ToString());
                }

                /*string sql = "SELECT [to].*, [co].[NameCourse] FROM [Topics] AS [to] INNER JOIN [Courses] AS [co] ON [to].[CourseId] = [co].[Id] WHERE [co].[DeletedAt] IS NULL";*/
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.NameTopic = reader["NameTopic"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
                        detail.Description = reader["Description"].ToString();
                        detail.NameVideo = reader["Video"].ToString();
                        detail.NameAudio = reader["Audio"].ToString();
                        detail.NameDocumentTopic = reader["DocumentTopic"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.viewCourseName = reader["NameCourse"].ToString();
                        topics.Add(detail);
                    }
                }
                connection.Close();

            }
            return topics;
        }

            public int InsetDataTopic(
                string nameTopic,
                int courseId,
                string? description,
                string videoTopic,
                string? audioTopic,
                string? documentTopic,
                string status
            )
            {

                int idTopic = 0;
                using (SqlConnection connection = Database.GetSqlConnection())
                {
                    string sqlQuery = "INSERT INTO [Topics]([NameTopic], [CourseId], [Description], [Video], [Audio], [DocumentTopic], [Status], [CreatedAt]) VALUES (@NameTopic,@CourseId,@Description,@Video,@Audio,@DocumentTopic,@Status,@CreatedAt) SELECT SCOPE_IDENTITY()";
                    //SELECT SCOPE_IDENTITY()  : lay ra ID vua moi them
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@NameTopic", nameTopic);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                    cmd.Parameters.AddWithValue("@Video", videoTopic);
                    cmd.Parameters.AddWithValue("@Audio", audioTopic);
                    cmd.Parameters.AddWithValue("@DocumentTopic", documentTopic);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    idTopic = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
                return idTopic;
            }

    }
}
