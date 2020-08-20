using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication_Kaido03
{
    public class UploadFileAttribute : ValidationAttribute
    {
        /// <summary>
        /// 許可する拡張子
        /// </summary>
        private string _extentions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="extentions">許可する拡張子。複数指定する場合はカンマで区切ること</param>
        public UploadFileAttribute(string extentions)
        {
            // デフォルトのエラーメッセージを設定する
            ErrorMessage = "{0}は{1}以外のファイルはアップロードできません。";

            this._extentions = extentions;
        }

        /// <summary>
        /// エラーメッセージを返却します。
        /// </summary>
        /// <param name="name">DisplayName の値</param>
        /// <returns>エラーメッセージ</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessage, name, _extentions);
        }

        /// <summary>
        /// ファイルの種類とサイズを検証します。
        /// </summary>
        /// <param name="value">ファイル</param>
        /// <returns>検証 OK の場合は true、検証 NG の場合は false</returns>
        public override bool IsValid(object value)
        {
            // 値が null の時、ファイルが指定されていない時はエラー
            if (value == null)
            {
                return false;
            }

            // 値がアップロードされたファイルか確認
            var postedFile = value as HttpPostedFileBase;

            if (postedFile == null)
            {
                return true;
            }

            // 拡張子を検証
            var extention = Path.GetExtension(postedFile.FileName).Replace(".", "");

            if (!string.IsNullOrEmpty(this._extentions) && !this._extentions.Split(',').Any(p => p == extention))
            {
                // 許可されていない拡張子の場合はエラー
                return false;
            }

            // 検証が成功
            return true;
        }
    }
}