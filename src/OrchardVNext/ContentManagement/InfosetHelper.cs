﻿using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using OrchardVNext.ContentManagement.FieldStorage.InfosetStorage;
using OrchardVNext.ContentManagement.Records;
using OrchardVNext.Utility;

namespace OrchardVNext.ContentManagement {
    public static class InfosetHelper {

        public static TProperty Retrieve<TPart, TProperty>(this TPart contentPart,
            Expression<Func<TPart, TProperty>> targetExpression,
            TProperty defaultValue = default(TProperty),
            bool versioned = false) where TPart : ContentPart {

            var propertyInfo = ReflectionHelper<TPart>.GetPropertyInfo(targetExpression);
            var name = propertyInfo.Name;

            var infosetPart = contentPart.As<InfosetPart>();
            var el = infosetPart == null
                ? null
                : (versioned ? infosetPart.VersionInfoset.Element : infosetPart.Infoset.Element)
                .Element(contentPart.GetType().Name);
            var attr = el != null ? el.Attribute(name) : default(XAttribute);
            return attr == null ? defaultValue : XmlHelper.Parse<TProperty>(attr.Value);
        }

        public static TProperty Retrieve<TProperty>(this ContentPart contentPart, string name,
            bool versioned = false) {
            var infosetPart = contentPart.As<InfosetPart>();
            var el = infosetPart == null
                ? null
                : (versioned ? infosetPart.VersionInfoset.Element : infosetPart.Infoset.Element)
                .Element(contentPart.GetType().Name);
            return el == null ? default(TProperty) : el.Attr<TProperty>(name);
        }

        public static void Store<TPart, TProperty>(this TPart contentPart,
            Expression<Func<TPart, TProperty>> targetExpression,
            TProperty value, bool versioned = false) where TPart : ContentPart {

            var partName = contentPart.GetType().Name;
            var infosetPart = contentPart.As<InfosetPart>();
            var propertyInfo = ReflectionHelper<TPart>.GetPropertyInfo(targetExpression);
            var name = propertyInfo.Name;

            Store(infosetPart, partName, name, value, versioned);
        }

        public static void Store<TProperty>(this ContentPart contentPart, string name,
            TProperty value, bool versioned = false) {

            var partName = contentPart.GetType().Name;
            var infosetPart = contentPart.As<InfosetPart>();

            Store(infosetPart, partName, name, value, versioned);
        }

        public static void Store<TProperty>(this InfosetPart infosetPart, string partName, string name, TProperty value, bool versioned = false) {

            var infoset = (versioned ? infosetPart.VersionInfoset : infosetPart.Infoset);
            var partElement = infoset.Element.Element(partName);
            if (partElement == null) {
                partElement = new XElement(partName);
                infoset.Element.Add(partElement);
            }
            partElement.Attr(name, value);
        }

        public static TProperty RetrieveValue<TDocumentRecord, TProperty>(this TDocumentRecord documentRecord,
           Expression<Func<TDocumentRecord, TProperty>> targetExpression,
           TProperty defaultValue = default(TProperty),
           bool versioned = false) where TDocumentRecord : DocumentRecord {

            var propertyInfo = ReflectionHelper<TDocumentRecord>.GetPropertyInfo(targetExpression);
            var name = propertyInfo.Name;

            var el = documentRecord == null
                ? null
                : (versioned ? documentRecord.VersionInfoset.Element : documentRecord.Infoset.Element)
                    .Element(documentRecord.GetType().Name);
            var attr = el != null ? el.Attribute(name) : default(XAttribute);
            return attr == null ? defaultValue : XmlHelper.Parse<TProperty>(attr.Value);
        }

        public static void StoreValue<TDocumentRecord, TProperty>(this TDocumentRecord documentRecord,
            Expression<Func<TDocumentRecord, TProperty>> targetExpression,
            TProperty value, bool versioned = false) where TDocumentRecord : DocumentRecord {

            var partName = documentRecord.GetType().Name;
            var propertyInfo = ReflectionHelper<TDocumentRecord>.GetPropertyInfo(targetExpression);
            var name = propertyInfo.Name;

            Store(documentRecord, partName, name, value, versioned);
        }

        public static void Store<TProperty>(this DocumentRecord documentRecord, string name,
            TProperty value, bool versioned = false) {

            var partName = documentRecord.GetType().Name;

            Store(documentRecord, partName, name, value, versioned);
        }

        public static void Store<TProperty>(this DocumentRecord documentRecord, string partName, string name, TProperty value, bool versioned = false) {

            var infoset = (versioned ? documentRecord.VersionInfoset : documentRecord.Infoset);
            var partElement = infoset.Element.Element(partName);
            if (partElement == null) {
                partElement = new XElement(partName);
                infoset.Element.Add(partElement);
            }
            partElement.Attr(name, value);
        }
    }
}
