﻿using Microsoft.AspNet.Mvc;
using OrchardVNext.ContentManagement;
using OrchardVNext.ContentManagement.Handlers;
using OrchardVNext.Data;
using OrchardVNext.Demo.Models;
using OrchardVNext.Test1;

namespace OrchardVNext.Demo.Controllers {
    public class HomeController : Controller {
        private readonly ITestDependency _testDependency;
        private readonly IContentStorageProvider _contentStorageProvider;
        private readonly IContentManager _contentManager;

        public HomeController(ITestDependency testDependency,
            IContentStorageProvider contentStorageProvider,
            IContentManager contentManager) {
            _testDependency = testDependency;
            _contentStorageProvider = contentStorageProvider;
            _contentManager = contentManager;
            }

        public ActionResult Index()
        {
            //var contentItem = new ContentItem
            //{
            //    VersionRecord = new ContentItemVersionRecord
            //    {
            //        ContentItemRecord = new ContentItemRecord(),
            //        Number = 1,
            //        Latest = true,
            //        Published = true
            //    }
            //};

            //contentItem.VersionRecord.ContentItemRecord.Versions.Add(contentItem.VersionRecord);

            //_contentStorageProvider.Store(contentItem);

            //var indexedRecordIds = _contentIndexProvider.GetByFilter(x => x.Id == 1);

            //var retrievedRecord = _contentStorageProvider.Get(contentItem.Id);

            //var indexedRetrievedRecords = _contentStorageProvider.GetMany(x => x.Id == 1);


            var contentItem = _contentManager.New("Foo");
            contentItem.As<TestContentPartA>().Line = "Orchard VNext Rocks";
            _contentManager.Create(contentItem);


            var retrieveContentItem = _contentManager.Get(contentItem.Id);
            var lineToSay = retrieveContentItem.As<TestContentPartA>().Line;


            return View("Index", _testDependency.SayHi(lineToSay));
        }
    }

    public class TestContentPartAHandler : ContentHandlerBase {
        public override void Activating(ActivatingContentContext context) {
            context.Builder.Weld<TestContentPartA>();
        }
    }
}