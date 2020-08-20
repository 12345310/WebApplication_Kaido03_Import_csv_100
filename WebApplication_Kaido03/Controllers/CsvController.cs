using System;
using System.Web;
using System.Web.Mvc;
using WebApplication_Kaido03.Services;
using System.Text;
using System.Web.UI.WebControls;
using WebApplication_Kaido03.Models;
using System.Linq;
using System.Collections.Generic;
using Sitecore.FakeDb;

namespace WebApplication_Kaido03.Controllers
{

    public class CsvController : Controller
    {

        private WebApplication_Kaido03Context db = new WebApplication_Kaido03Context();

        // GET: Csv
        public ActionResult Index()
        {
            return RedirectToAction("Import");
        }

        // GET: Parents/Import
        public ActionResult Import()
        {
            return View();
        }

        // POST: Parents/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(CsvFile file)
        {
            // ファイルチェック
            if (!ModelState.IsValid)
            {
                return View();
            }

            // ファイル読み込み
            CsvImportService csvImportService = new CsvImportService(file.UploadFile.InputStream);

            // バリデーション
            if (!csvImportService.IsValid)
            {
                ViewBag.ErrorMessageList = csvImportService.ErrorMessageList;
                return View();
            }

            // モデル取得
            List<Parent> parentList = csvImportService.ParentList;

            // DB 登録
            parentList.ForEach(p => db.Parents.Add(p));
            db.SaveChanges();

            ViewBag.SuccessMessage = "インポートに成功しました。";
            return View();
        }
    }
}