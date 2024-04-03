using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhamThanhXuan;

namespace PhamThanhXuan
{
    using System;
    using System.ComponentModel;
    using System.Net.Http.Headers;
    using System.Runtime.InteropServices;
    using System.Xml;

    public class DataInput
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public DataInput(string name, int priority, string description, string status)
        {
            Name = name;
            Priority = priority;
            Description = description;
            Status = status;
        }

        public void ShowDatainput()
        {
            Console.WriteLine("Tên công việc: " + Name);
            Console.WriteLine("Độ ưu tiên: " + Priority);
            Console.WriteLine("Mô tả công việc: " + Description);
            Console.WriteLine("Trạng thái: " + Status);
        }

        public static DataInput InputData(List<DataInput> dataLists)
        {
            while (true)
            {
                Console.Write("Tên công việc:");
                string name = Console.ReadLine();

                bool isDuplicate = false;
                foreach (DataInput task in dataLists)
                {
                    if (task.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Công việc với tên đã tồn tại trong danh sách. Vui lòng nhập lại.");
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                    continue;

                Console.Write("Độ ưu tiên (số nguyên từ 1 đến 5):");
                int priority;
                while (!int.TryParse(Console.ReadLine(), out priority) || priority < 1 || priority > 5)
                {
                    Console.Write("Mức độ ưu tiên từ 1 đến 5. Vui lòng nhập lại:");
                }

                Console.Write("Mô tả công việc:");
                string description = Console.ReadLine();

                Console.Write("Trạng thái công việc:");
                string status = Console.ReadLine();

                return new DataInput(name, priority, description, status);
            }
        }
    }


    internal class Program
    {
        static List<DataInput> DataLists = new List<DataInput>();
        static List<DataInput> TemplDataLists = new List<DataInput>(DataLists);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            while (true)
            {
                Console.WriteLine("1. Khai báo thông tin các việc cần làm");
                Console.WriteLine("2. Xóa thông tin việc cần làm");
                Console.WriteLine("3. Cập nhật trạng thái dựa vào tên việc cần làm.");
                Console.WriteLine("4. Hỗ trợ tìm kiếm việc cần dựa vào tên hoặc độ ưu tiên.");
                Console.WriteLine("5. Hiển thị danh sách các việc cần làm theo độ ưu tiên giảm dần");
                Console.WriteLine("6. Hiển thị toàn bộ danh sách việc cần làm mà người dùng đã khai báo");
                Console.WriteLine("7. Hiển thị toàn bộ danh sách việc người dùng đã khai báo");
                Console.WriteLine();
                Console.Write("Chọn mục yêu cầu: ");
                int Num = int.Parse(Console.ReadLine());
                switch (Num)
                {
                    case 1:
                        AddDatatoList(); break;
                    case 2:
                        DeleteData(); break;
                    case 3:
                        // Nhập tên công việc muốn cập nhật trạng thái
                        Console.WriteLine();
                        Console.Write("Nhập tên công việc muốn cập nhật trạng thái: ");
                        string taskNameToUpdateStatus = Console.ReadLine();

                        // Nhập trạng thái mới
                        Console.Write("Nhập trạng thái mới: ");
                        string newStatus = Console.ReadLine();
                        UpdataStatusbasedName(taskNameToUpdateStatus, newStatus); break;
                    case 4:
                        Search();
                        break;
                    case 5:
                        SortFromSmailltoLarrge();
                        break;
                    case 6:
                        foreach (DataInput tasklist in DataLists)
                        {
                            tasklist.ShowDatainput();
                            Console.WriteLine();
                        }
                        break;

                }
                Console.WriteLine();
            }
        }

        private static void AddDatatoList()
        {
            Console.WriteLine("Nhập thông tin cho các công việc:");

            while (true)
            {
                DataInput tasklist = DataInput.InputData(DataLists);
                DataLists.Add(tasklist);
                Console.WriteLine();
                Console.Write("Bạn có muốn nhập tiếp công việc khác không? nhấn Y: ");
                string input = Console.ReadLine();
                if (input.ToLower() != "y")
                    break;

                Console.WriteLine();
            }
            Console.WriteLine("Danh sách công việc:");
            //foreach (DataInput tasklist in DataLists)
            //{
            //    tasklist.ShowDatainput();
            //    Console.WriteLine();
            //}

        }

        private static void DeleteData()
        {
            Console.WriteLine("Nhập số thứ tự công việc bạn muốn xóa đi:");
            int Index;

            while (!int.TryParse(Console.ReadLine(), out Index))
            {
                Console.Write("Bạn phải nhập một số nguyên: ");
            }

            while (Index < 1 || Index > DataLists.Count)
            {
                Console.Write("Danh sách công việc chỉ có {0}. Vui lòng nhập lại: ", DataLists.Count);
                while (!int.TryParse(Console.ReadLine(), out Index))
                {
                    Console.Write("Bạn phải nhập một số nguyên: ");
                }
            }

            for (int i = 0; i < DataLists.Count; i++)
            {
                if (i == Index - 1)
                {
                    DataLists.RemoveAt(i);
                    break;
                }
            }
            //Console.WriteLine("Danh sách công việc sau khi xóa:");
            //foreach (DataInput tasklist in DataLists)
            //{
            //    tasklist.ShowDatainput();
            //    Console.WriteLine();
            //}
        }

        private static void UpdataStatusbasedName(string taskName, string newStatus)
        {
            DataInput findName = null;
            foreach (DataInput tasklist in DataLists)
            {
                if (tasklist.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase)) //StringComparison.OrdinalIgnoreCase không phẩn biệt chữ thường or hooa
                {
                    findName = tasklist;
                    break;
                }
            }
            if (findName != null)
            {
                findName.Status = newStatus;
            }
            else
            {
                Console.WriteLine($"Không tìm thấy công việc có tên '{taskName}'.");
            }
            //Console.WriteLine("Danh sách công việc đã updated:");
            //foreach (DataInput tasklist in DataLists)
            //{
            //    tasklist.ShowDatainput();
            //    Console.WriteLine();
            //}
        }

        #region Tim kiem gia trị theo yêu cầu
        private static void Search()
        {
            while (true)
            {
                Console.WriteLine("1. Tìm kiếm theo tên");
                Console.WriteLine("2. Tìm kiếm theo độ ưu tiên");
                Console.WriteLine("3. Thoát");
                Console.Write("Chọn loại tìm kiếm:");
                string valuechoice = Console.ReadLine();

                switch (valuechoice)
                {
                    case "1":
                        Console.Write("Nhập tên công việc cần tìm: ");
                        string TextName = Console.ReadLine();
                        List<DataInput> NameList = SearchName(TextName);
                        DisplaySearchResults(NameList);
                        break;
                    case "2":
                        Console.Write("Nhập độ ưu tiên cần tìm: ");
                        int valuePriority;
                        while (!int.TryParse(Console.ReadLine(), out valuePriority))
                        {
                            Console.WriteLine("Độ ưu tiên phải là một số nguyên.");
                            Console.Write("Nhập độ ưu tiên cần tìm: ");
                        }
                        List<DataInput> PriorityList = SearchPriority(valuePriority);
                        DisplaySearchResults(PriorityList);
                        break;
                    case "3":
                        Console.WriteLine("Thoát chương trình.");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        continue;
                }
            }
        }

        public static List<DataInput> SearchName(string nameSearch)
        {
            List<DataInput> result = new List<DataInput>();

            foreach (DataInput item in DataLists)
            {
                if (item.Name.ToLower().Contains(nameSearch.ToLower()))
                {
                    result.Add(item);
                }
            }

            return result;
        }


        private static List<DataInput> SearchPriority(int prioritySearch)
        {
            List<DataInput> result = new List<DataInput>();

            foreach (DataInput items in DataLists)
            {
                if (items.Priority == prioritySearch)
                {
                    result.Add(items);
                }
            }

            return result;
        }


        static void DisplaySearchResults(List<DataInput> tasks)
        {
            if (tasks.Count > 0)
            {
                Console.WriteLine("Công việc được tìm thấy:");
                foreach (var task in tasks)
                {
                    task.ShowDatainput();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy công việc phù hợp.");
            }
        }
        #endregion

        #region Sort
        private static void SortFromSmailltoLarrge()
        {
            while (true)
            {
                Console.WriteLine("1. Sắp từ nhỏ đến bé");
                Console.WriteLine("2. Trả lại list ban đầu");
                Console.WriteLine("3. Thoát");
                Console.Write("Chọn loại tìm kiếm:");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Danh sách từ nhỏ đến lớn ");
                        DisplaySort();
                        break;
                    case "2":
                        Console.Write("Done");
                        DataLists = new List<DataInput>(TemplDataLists);
                        break;
                    case "3":
                        Console.WriteLine("Thoát chương trình.");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        continue;
                }
            }
        }
        private static void DisplaySort()
        {
            if (DataLists.Count > 0)
            {
                SortByPriority();

                Console.WriteLine("Công việc được tìm thấy:");
                foreach (var task in DataLists)
                {
                    task.ShowDatainput();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy công việc phù hợp.");
            }
        }

        private static void SortByPriority()
        {
            int n = DataLists.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int maxIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (DataLists[j].Priority == DataLists[maxIndex].Priority) // Nếu cùng giá trị thì giữ nguyên
                    {
                        continue;
                    }
                    if (DataLists[j].Priority > DataLists[maxIndex].Priority) // Nếu lớn hơn thì thay thế j cho max
                    {
                        maxIndex = j;
                    }
                }

                DataInput temp = DataLists[maxIndex];
                DataLists[maxIndex] = DataLists[i];
                DataLists[i] = temp;
            }   
        }
    }
    #endregion
}


