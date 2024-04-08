using Microsoft.Data.SqlClient;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TrainingFPT.Models.Queries
{
    public class TopicQuery
    {
        public bool UpdateTopicById(
            string nameTopic,
            int courseId,
            string? description,
            string video,
            string audio,
            string documentTopic,
            string status,
            int id
        )
        {
            bool checkingUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Topics] SET [NameTopic] = @NameTopic, [CourseId] = @CourseId, [Description] = @Description, [Video] = @Video, [Audio] = @Audio, [DocumentTopic] = @DocumentTopic, [Status] = @Status, [UpdatedAt] = @UpdatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@NameTopic", nameTopic);
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Video", video ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Audio", audio ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@DocumentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                checkingUpdate = true;
                connection.Close();
            }
            return checkingUpdate;
        }


        public bool DeleteItemTopic(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] =  @deletedAt WHERE [Id] = @id";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                statusDelete = true;
                connection.Close();
            }
            //false: ko xoa dc --- true : xoa thanh cong
            return statusDelete;
        }


        public TopicDetail GetDetailTopicById(int id)
        {
            TopicDetail topic = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topic.Id = Convert.ToInt32(reader["Id"]);
                        topic.NameTopic = reader["NameTopic"].ToString();
                        topic.CourseId = Convert.ToInt32(reader["CourseId"]);
                        topic.Description = reader["Description"].ToString();
                        topic.Status = reader["Status"].ToString();
                        topic.NameVideo = reader["Video"].ToString();
                        topic.NameAudio = reader["Audio"].ToString();
                        topic.NameDocumentTopic = reader["DocumentTopic"].ToString();
                    }
                }
                connection.Close();
            }
            return topic;
        }


        public List<TopicDetail> GetAllDataTopics(string? keyword)
        {
            string search = "%" + keyword + "%";
            List<TopicDetail> topics = new List<TopicDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [co].*, [ca].[NameCourse] FROM [Topics] AS [co] INNER JOIN [Courses] AS [ca] ON [co].[CourseId] = [ca].[Id] WHERE ([co].[NameTopic] LIKE @NameTopic OR [ca].[NameCourse] LIKE @NameCourse OR [co].[Description] LIKE @Description) AND [co].[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@NameTopic", search ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@NameCourse", search ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Description", search ?? DBNull.Value.ToString());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TopicDetail detail = new TopicDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.NameTopic = reader["NameTopic"].ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CourseId = Convert.ToInt32(reader["CourseId"]);
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
            string status,
            string videoTopic,
            string audioTopic,
            string documentTopic
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
