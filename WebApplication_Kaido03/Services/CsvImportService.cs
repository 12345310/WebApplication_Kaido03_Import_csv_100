using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication_Kaido03.Models;

namespace WebApplication_Kaido03.Services
{
    public class CsvImportService
    {
        private WebApplication_Kaido03Context db = null;

        /// <summary>
        /// 性別マスタデータ
        /// </summary>
        private List<Sex> sexList = null;

        /// <summary>
        /// CSV ファイルの最低行数
        /// </summary>
        private const int MIN_CSV_ROW_COUNT = 2;

        /// <summary>
        /// CSV 1 行に含める Child の最大数
        /// </summary>
        private const int MAX_CHILD_COUNT = 2;

        /// <summary>
        /// ヘッダー定義
        /// </summary>
        private string[] headers =
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
        /// CSV 1行における Parent 1 つを構成するカラム数
        /// </summary>
        private const int ONE_PARENT_COLUMN_COUNT = 3;

        /// <summary>
        /// CSV 1行における Child 1 つを構成するカラム数
        /// </summary>
        private const int ONE_CHILD_COLUMN_COUNT = 3;

        /// <summary>
        /// 読みこんだ CSV のバリデーションが OK であることを示すフラグ
        /// </summary>
        public bool IsValid = false;

        /// <summary>
        /// エラーメッセージリスト
        /// </summary>
        public List<string> ErrorMessageList = new List<string>();

        /// <summary>
        /// Parent モデルのリスト。IsValid プロパティが false の場合は内容の有効性が保証されない。
        /// </summary>
        public List<Parent> ParentList = new List<Parent>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="inputStream">CSV ファイルの Stream</param>
        public CsvImportService(Stream inputStream)
        {
            this.db = new WebApplication_Kaido03Context();
            this.sexList = this.db.Sexes.ToList();

            IEnumerable<string[]> csvLines = ReadCsv(inputStream);
            // TODO 2回目の LINQ 取得がうまくいかないので for する。
            //var header = csvLines.FirstOrDefault();
            //var lineList = csvLines.Skip(0).ToList();
            int i = 0;
            foreach (var csvLine in csvLines)
            {
                // 1回目のみヘッダーチェック
                i++;
                if (i == 1)
                {
                    ValidateHeader(csvLine);
                    continue;
                }

                // 2回目以降はチェックおよびオブジェクト生成
                ValidateLineAndMakeParent(i, csvLine);
            }

            // 2回目以降はチェックおよびオブジェクト生成
            if (i < MIN_CSV_ROW_COUNT)
            {
                ErrorMessageList.Add("読み込むデータがありません。");
            }

            IsValid = (ErrorMessageList.Count == 0);
        }

        /// <summary>
        /// CSV の Stream を読み込み、string 配列の列挙子を返却します。
        /// </summary>
        /// <param name="stream">CSV ファイルの Stream</param>
        /// <returns>各セルを string 配列の要素とし、1 行を 1 つの要素とした列挙子</returns>
        private IEnumerable<string[]> ReadCsv(Stream stream)
        {
            using (TextFieldParser parser = new TextFieldParser(stream, Encoding.GetEncoding("Shift_JIS")))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(new[] { "," });
                parser.HasFieldsEnclosedInQuotes = true;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    yield return fields;
                }
            }
        }

        /// <summary>
        /// CSV のヘッダーを検証し、NG の項目はエラーメッセージに追加します。
        /// </summary>
        /// <param name="csvHeader">CSV ヘッダー項目の配列</param>
        private void ValidateHeader(string[] csvHeader)
        {
            // 要素数チェック
            if (this.headers.Length != csvHeader.Length)
            {
                IsValid = false;
                ErrorMessageList.Add("ヘッダーの要素数が一致しません。");
            }

            // 値チェック
            int max = csvHeader.Length;
            for (int i = 0; i < max; i++)
            {
                if (headers[i] != csvHeader[i])
                {
                    IsValid = false;
                    ErrorMessageList.Add("ヘッダー[ " + headers[i] + " ]の値が一致しません。");
                }
            }
        }

        /// <summary>
        /// 1行分 の CSV を検証し、Parent および Child インスタンスを生成して ParentList プロパティに追加します。
        /// 検証 NG の場合はエラーメッセージをプロパティに追加します。
        /// 検証 NG の場合でもインスタンスを生成しますが、内容は保証されません。
        /// </summary>
        /// <param name="rowNumber">行番号</param>
        /// <param name="rowContent">1 行分の CSV 要素配列</param>
        private void ValidateLineAndMakeParent(int rowNumber, string[] rowContent)
        {
            int columnNumber = 0;

            var p = new Parent();
            // 親_名前
            p.Name = ValidateAndGetRequiredString(rowNumber, columnNumber++, rowContent);
            // 親_性別
            p.SexId = ValidateAndGetRequiredSexId(rowNumber, columnNumber++, rowContent);
            // 親_メールアドレス
            p.Email = ValidateAndGetRequiredString(rowNumber, columnNumber++, rowContent);
            p.Children = new List<Child>();

            var childCount = GetChildCountInRow(rowContent.Length);
            for (var i = 0; i < childCount; i++)
            {
                if (i == MAX_CHILD_COUNT)
                {
                    break;
                }
                var c = new Child();
                // 子n_名前
                c.Name = ValidateAndGetRequiredString(rowNumber, columnNumber++, rowContent);
                // 子n_性別
                c.SexId = ValidateAndGetRequiredSexId(rowNumber, columnNumber++, rowContent);
                // 子n_生年月日
                c.Birthday = ValidateAndGetRequiredDateTime(rowNumber, columnNumber++, rowContent);
                p.Children.Add(c);
            }

            ParentList.Add(p);
        }

        /// <summary>
        /// 列番号に対応した要素の値を検証し、OK の場合は値をそのまま返却します。
        /// 検証 NG の場合は行数を含めてエラーメッセージをプロパティに追加し、null を返却します。
        /// </summary>
        /// <param name="rowNumber">行数</param>
        /// <param name="columnNumber">列番号</param>
        /// <param name="rowContent">1 行分の CSV 要素配列</param>
        /// <returns>列番号に対応した要素の値。検証 NG の場合は null</returns>
        private string ValidateAndGetRequiredString(int rowNumber, int columnNumber, string[] rowContent)
        {
            var value = GetStringOrNull(rowContent, columnNumber);

            if (string.IsNullOrEmpty(value))
            {
                ErrorMessageList.Add(rowNumber + "行目 : [ " + headers[columnNumber] + " ] は必須です。");
                return "";
            }

            return value;
        }

        /// <summary>
        /// インデックスに対応した配列の要素を返却します。
        /// インデックスが配列の要素数の範囲外の場合は null を返却します。
        /// </summary>
        /// <param name="array">string の配列</param>
        /// <param name="index">配列のインデックス</param>
        /// <returns>インデックスに対応した配列の要素。無い場合は null</returns>
        private string GetStringOrNull(string[] array, int index)
        {
            if (array.Length <= index)
            {
                return null;
            }

            return array[index];
        }

        /// <summary>
        /// 列番号に対応した要素の値を検証し、OK の場合は要素の値に対応する SexId を返却します。
        /// </summary>
        /// <param name="rowNumber">行数</param>
        /// <param name="columnNumber">列番号</param>
        /// <param name="rowContent">1 行分の CSV 要素配列</param>
        /// <returns>SexId。検証 NG の場合は 0</returns>
        private int ValidateAndGetRequiredSexId(int rowNumber, int columnNumber, string[] rowContent)
        {
            var value = GetStringOrNull(rowContent, columnNumber);

            if (string.IsNullOrEmpty(value))
            {
                ErrorMessageList.Add(rowNumber + "行目 : [ " + headers[columnNumber] + " ] は必須です。");
                return 0;
            }

            if (!sexList.Any(s => s.Name == value))
            {
                ErrorMessageList.Add(rowNumber + "行目 : [ " + headers[columnNumber] + " ] の入力値は誤っています。");
                return 0;
            }

            return sexList.Single(s => s.Name == value).Id;
        }

        /// <summary>
        /// CSV 1 行に含まれる子の数を算出します。
        /// 1 行の要素数が少ない場合、この数はマイナスや 0 となる場合があります。
        /// </summary>
        /// <param name="rowColumnCount">CSV  1 行の要素数</param>
        /// <returns>CSV 1 行に含まれる子の数。</returns>
        private int GetChildCountInRow(int rowColumnCount)
        {
            return (rowColumnCount - ONE_PARENT_COLUMN_COUNT) / ONE_CHILD_COLUMN_COUNT;
        }

        /// <summary>
        /// 列番号に対応した要素の値を検証し、OK の場合は要素の値に対応する DateTime を返却します。
        /// </summary>
        /// <param name="rowNumber">行数</param>
        /// <param name="columnNumber">列番号</param>
        /// <param name="rowContent">1 行分の CSV 要素配列</param>
        /// <returns>DateTime に変換した年月日。検証 NG の場合は DateTime.MinValue</returns>
        private DateTime ValidateAndGetRequiredDateTime(int rowNumber, int columnNumber, string[] rowContent)
        {
            var value = GetStringOrNull(rowContent, columnNumber);

            if (string.IsNullOrEmpty(value))
            {
                ErrorMessageList.Add(rowNumber + "行目 : [ " + headers[columnNumber] + " ] は必須です。");
                return DateTime.MinValue;
            }

            DateTime result;
            if (!DateTime.TryParse(value, out result))
            {
                ErrorMessageList.Add(rowNumber + "行目 : [ " + headers[columnNumber] + " ] の入力値は誤っています。");
            }
            return result;
        }
    }
}