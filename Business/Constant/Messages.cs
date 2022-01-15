using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constant
{
    public static class Messages
    {
        public static string MaxCategory = "The category already has max number of the product ";
        public static string ProductAdded = "Product Added";
        public static string ProductNameInvalid = "Product name invalid";
        public static string MaintenanceTime = "System currently maintenanced";
        public static string ProductListed = "System currently maintenanced";
        public static string CategoryOutofLimit = "Category Out Of limit";
        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string UserAlreadyExists = "Kullanıcı zaten bulunmakta";
        public static string UserRegistered = "Kullanıcı kaydedildi";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Kullanıcı adı ya da parola yanlış";
        public static string SuccessfulLogin = "Giriş yapıldı";
        public static string AccessTokenCreated = "Token Oluşturuldu";

    }
}
