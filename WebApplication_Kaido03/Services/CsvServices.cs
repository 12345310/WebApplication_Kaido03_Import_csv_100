using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication_Kaido03.Models;

namespace WebApplication_Kaido03.Services
{
    public class CsvServices
    {
        /// <summary>
        /// CSV セルの開始終了マーク
        /// </summary>
        private const string ENCLOSE_CHARACTER = "\"";

        /// <summary>
        /// CSV セル区切り文字
        /// </summary>
        private const string DELIMITER = ",";

        /// <summary>
        /// CSV ヘッダー要素の配列
        /// </summary>
        private static readonly string[] HEADER_ARRAY = new string[]
        {
            "親_名前",
            "親_性別",
            "親_メールアドレス",
            "子1_名前",
            "子1_性別",
            "子1_生年月日",
            "子2_名前",
            "子2_性別",
            "子2_生年月日"
        };

        /// <summary>
        /// CSV 1 行に付加する Child の数
        /// </summary>
        private const int ADDED_CHILDREN_NUMBER = 3;

        /// <summary>
        /// CSV ファイルデータの文字列を生成して返却します。
        /// </summary>
        /// <param name="parentList">Parent オブジェクトの List</param>
        /// <returns>CSV ファイルデータの文字列</returns>
        public static string CreateCsv(List<Parent> parentList)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetCsvHeader());
            parentList.ForEach(p => sb.AppendLine(CreateCsvLine(p)));
            return sb.ToString();
        }

        /// <summary>
        /// CSV ヘッダ定義文字列を返却します。
        /// 改行文字は含みません。
        /// </summary>
        /// <returns>CSV ヘッダ定義文字列</returns>
        private static string GetCsvHeader()
        {
            var sb = new StringBuilder();
            foreach (var v in HEADER_ARRAY)
            {
                sb.Append(ENCLOSE_CHARACTER).Append(v).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
            }
            // 最後のデリミタを削除して返却
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// CSV の 1 行の文字列を生成して返却します。
        /// Child は 2 つまで含め、3 つ目以降は返却文字列に付加されません。
        /// 改行文字は含みません。
        /// </summary>
        /// <param name="parent">Parent オブジェクト</param>
        /// <returns></returns>
        private static string CreateCsvLine(Parent parent)
        {
            var sb = new StringBuilder();
            sb.Append(ENCLOSE_CHARACTER).Append(parent.Name).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
            sb.Append(ENCLOSE_CHARACTER).Append(parent.Sex.Name).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
            sb.Append(ENCLOSE_CHARACTER).Append(parent.Email).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
            // ヘッダ定義で子どもは 2 人までと決めたので、同じクラスで CSV 出力対象とする人数を制限する。
            var i = 0;
            foreach (var child in parent.Children)
            {
                i++;
                if (i == ADDED_CHILDREN_NUMBER) break;

                sb.Append(ENCLOSE_CHARACTER).Append(child.Name).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
                sb.Append(ENCLOSE_CHARACTER).Append(child.Sex.Name).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
                sb.Append(ENCLOSE_CHARACTER).Append(child.Birthday.ToShortDateString()).Append(ENCLOSE_CHARACTER).Append(DELIMITER);
            }
            // 最後のデリミタを削除して返却
            return sb.Remove(sb.Length - 1, 1).ToString();

        }
    }
}