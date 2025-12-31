using System.Data;
using System.Reflection;
namespace courseService.Utils
{
    public class DatatableHelper
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();

            if (dt == null || dt.Rows.Count == 0)
            {
                return data;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                T item = Activator.CreateInstance<T>();

                foreach (PropertyInfo prop in properties)
                {
                    // Kiểm tra xem cột trong DB có tồn tại không
                    if (dt.Columns.Contains(prop.Name))
                    {
                        object value = row[prop.Name];

                        // Nếu giá trị khác NULL trong DB thì mới xử lý
                        if (value != DBNull.Value)
                        {
                            // --- ĐOẠN NÀY LÀ CHÌA KHÓA NÈ ---

                            // 1. Kiểm tra xem property của mày có phải là Nullable (vd: DateTime?) không?
                            // Nếu có, lấy kiểu gốc (DateTime). Nếu không, giữ nguyên.
                            Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            try
                            {
                                // 2. Ép kiểu sang kiểu gốc (targetType) thay vì ép sang Nullable
                                object safeValue = Convert.ChangeType(value, targetType);

                                // 3. Gán giá trị vào object
                                prop.SetValue(item, safeValue);
                            }
                            catch
                            {
                                // Nếu ép kiểu lỗi (ví dụ DB lưu chuỗi "abc" mà code là int)
                                // Thì bỏ qua hoặc log lỗi, không crash app
                                continue;
                            }
                        }
                    }
                }
                data.Add(item);
            }
            return data;
        }
    }
}