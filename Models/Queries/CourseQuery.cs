using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class CourseQuery
    {
        public bool UpdateCourseById(
            string nameCourse,
            int categoryId,
            string? description,
            string image,
            string status,
            DateTime startDate,
            DateTime? endDate,
            int id
        )
        {
            bool checkingUpdate = false;

            string valEndDate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEndDate = endDate.Value.ToString();
            }

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "UPDATE [Courses] SET [CategoryId] = @CategoryId, [NameCourse] = @NameCourse, [Description] = @Description, [Image] = @Image, [Status] = @Status, [StartDate] = @StartDate, [EndDate] = @EndDate, [UpdatedAt] = @UpdatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand( sql, connection );
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", image ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEndDate);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                checkingUpdate = true;
                connection.Close();
            }
            return checkingUpdate;
        }


        public CourseDetail GetDetailCourseById(int id)
        {
            CourseDetail course = new CourseDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT * FROM [Courses] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        course.Id = Convert.ToInt32(reader["Id"]);
                        course.NameCourse = reader["NameCourse"].ToString();
                        course.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        course.Description = reader["Description"].ToString();
                        course.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        course.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        course.Status = reader["Status"].ToString();
                        course.ViewImageCourse = reader["Image"].ToString();
                    }
                }
                connection.Close();
            }
            return course;
        }



        public List<CourseDetail> GetAllDataCourses(string? keyword)
        {
            string search = "%" + keyword + "%";
            List<CourseDetail> courses = new List<CourseDetail>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sql = "SELECT [co].*, CONVERT(VARCHAR(50), [co].[StartDate], 101) AS ViewStartDate, CONVERT(VARCHAR(50), [co].[EndDate], 101) AS ViewEndDate, [ca].[Name] FROM [Courses] AS [co] INNER JOIN [Categories] AS [ca] ON [co].[CategoryId] = [ca].[Id] WHERE ([co].[NameCourse] LIKE @NameCourse OR [ca].[Name] LIKE @NameCategory OR [co].[Description] LIKE @Description) AND [co].[DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@NameCourse", search ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@NameCategory", search ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Description", search ?? DBNull.Value.ToString());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        CourseDetail detail = new CourseDetail();
                        detail.Id = Convert.ToInt32(reader["Id"]);
                        detail.NameCourse = reader["NameCourse"].ToString();
                        detail.Description = reader["Description"].ToString();
                        detail.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        detail.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        detail.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        detail.ViewImageCourse = reader["Image"].ToString();
                        detail.Status = reader["Status"].ToString();
                        detail.viewCategoryName = reader["Name"].ToString();
                        detail.ViewStartDate =reader["ViewStartDate"].ToString();
                        detail.ViewEndDate =reader["ViewEndDate"].ToString();
                        courses.Add(detail);
                    }   
                }
                connection.Close();
            }
            return courses;
        }

        //viet method xoa course
        public bool DeleteItemCourse(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Courses] SET [DeletedAt] =  @deletedAt WHERE [Id] = @id";
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

        public int InsetDataCourse(
            string nameCourse,
            int categoryId,
            string? description,
            DateTime startDate,
            DateTime? endDate,
            string status,
            string imageCourse
        )
        {
            string valEnddate = DBNull.Value.ToString();
            if (endDate != null)
            {
                valEnddate = endDate.Value.ToString();
            }


            int idCourse = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Courses]([CategoryId], [NameCourse], [Description], [Image], [Status], [StartDate], [EndDate], [CreatedAt]) VALUES (@CategoryId,@NameCourse,@Description,@Image,@Status,@StartDate,@EndDate,@CreatedAt) SELECT SCOPE_IDENTITY()";
                //SELECT SCOPE_IDENTITY()  : lay ra ID vua moi them
                connection.Open();
                SqlCommand cmd = new SqlCommand( sqlQuery, connection );
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@NameCourse", nameCourse);
                cmd.Parameters.AddWithValue("@Description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@Image", imageCourse);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", valEnddate);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                idCourse = Convert.ToInt32( cmd.ExecuteScalar() );
                connection.Close();
            }
            return idCourse;
        }

    }
}
