// using System;
// using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;
// using UnityEngine;
//
// public class SaveSystem : MonoBehaviour
// {
//     private const string FileType = ".save";
//     private static string SavePath {
//         get { return Application.persistentDataPath + "/Saves/"; }
//     }
//
//     private static string BackUpSavePath {
//         get { return Application.persistentDataPath + "/BackUps/"; }
//     }
//
//     private static int SaveCount;
//     
//     public static void SaveData<T>(T data, string fileName)
//     {
//         Directory.CreateDirectory(SavePath);
//         Directory.CreateDirectory(BackUpSavePath);
//
//         if (SaveCount % 5 == 0) Save(BackUpSavePath);
//         Save(SavePath);
//
//         SaveCount++;
//         
//         void Save(string path)
//         {
//             using (StreamWriter writer = new StreamWriter(path + fileName + FileType))
//             {
//                 BinaryFormatter formatter = new BinaryFormatter();
//                 MemoryStream memoryStream = new MemoryStream();
//                 formatter.Serialize(memoryStream, data);
//                 string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
//                 writer.WriteLine(dataToSave);
//             }
//         }
//     }
//
//     public static T LoadData<T>(string fileName)
//     {
//         Directory.CreateDirectory(SavePath);
//         Directory.CreateDirectory(BackUpSavePath);
//
//         bool backUpNeeded = false;
//         T dataToReturn;
//         
//         Load(SavePath);
//         if (backUpNeeded) Load(BackUpSavePath);
//
//         return dataToReturn;
//
//         void Load(string path) {
//             Debug.Log("loading");
//             using (StreamReader reader = new StreamReader(path + fileName + FileType))
//             {
//                 BinaryFormatter formatter = new BinaryFormatter();
//                 string dataToLoad = reader.ReadToEnd();
//                 MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToLoad));
//
//                 try { dataToReturn = (T)formatter.Deserialize(memoryStream); }
//                 catch {
//                     backUpNeeded = true;
//                     dataToReturn = default;
//                 }
//             }
//         }
//     }
//
//     public static bool SaveExists(string fileName) {
//         return File.Exists(SavePath + fileName + FileType)
//                || File.Exists(BackUpSavePath + fileName + FileType);
//     }
//
//     // If File.Exists gives an error, try System.IO.File
// }

using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public static class SimpleAES {
    // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
    // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
    private const string initVector = "gwagwa6942069420";

    // This constant is used to determine the keysize of the encryption algorithm
    private const int keysize = 256;

    //Encrypt
    public static string EncryptString(string plainText, string passPhrase) {
        var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        var password = new PasswordDeriveBytes(passPhrase, null);
        var keyBytes = password.GetBytes(keysize / 8);
        var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        var memoryStream = new MemoryStream();
        var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        var cipherTextBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return Convert.ToBase64String(cipherTextBytes);
    }

    //Decrypt
    public static string DecryptString(string cipherText, string passPhrase) {
        var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        var cipherTextBytes = Convert.FromBase64String(cipherText);
        var password = new PasswordDeriveBytes(passPhrase, null);
        var keyBytes = password.GetBytes(keysize / 8);
        var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
        var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainTextBytes = new byte[cipherTextBytes.Length];
        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    }
}

public class SaveSystem : MonoBehaviour {
    static string encryptKey = "1gff823jsjg7h2f89hj89gwa";
    static string savePath => Application.persistentDataPath + "/saves/";
    static string savePathBackUP => Application.persistentDataPath + "/backups/";

    static int backUpCount;

    static string saveFileExtension = ".save";

    public static void SaveData<T>(T data, string saveToPath, bool useEncryption = true) {
        Directory.CreateDirectory(savePath);
        Directory.CreateDirectory(savePathBackUP);
        backUpCount++;
        if (backUpCount % 4 == 0) {
            // Debug.Log("backup save");
            Save(savePathBackUP);
            Save(savePath);
        }
        else {
            Save(savePath);
        }

        PlayerPrefs.SetString("OfflineTime", DateTime.Now.ToBinary().ToString());

        void Save(string path) {
            using (var writer = new StreamWriter(path + saveToPath + saveFileExtension)) {
                var formatter = new BinaryFormatter();
                var memoryStream = new MemoryStream();
                formatter.Serialize(memoryStream, data);
                var dataToWrite = useEncryption 
                    ? SimpleAES.EncryptString(Convert.ToBase64String(memoryStream.ToArray()), encryptKey) 
                    : Convert.ToBase64String(memoryStream.ToArray());
                writer.WriteLine(dataToWrite);
            }
        }

        Debug.Log($"data saved! {DateTime.Now.ToLongTimeString()}");
    }

    public static T LoadData<T>(string name, bool useEncryption = true) {
        Directory.CreateDirectory(savePath);
        Directory.CreateDirectory(savePathBackUP);
        T returnValue;
        var backUpNeeded = false;

        Load(savePath);
        if (backUpNeeded) {
            Debug.Log("backup load");
            Load(savePathBackUP);
        }

        void Load(string path) {
            using (var reader = new StreamReader(path + name + saveFileExtension)) {
                var formatter = new BinaryFormatter();
                var dataToRead = reader.ReadToEnd();
                var memoryStream = useEncryption
                    ? new MemoryStream(Convert.FromBase64String(SimpleAES.DecryptString(dataToRead, encryptKey)))
                    : new MemoryStream(Convert.FromBase64String(dataToRead));
                try {
                    returnValue = (T) formatter.Deserialize(memoryStream);
                }
                catch {
                    backUpNeeded = true;
                    returnValue = default;
                }
            }
        }
        
        return returnValue;
    }

    public static bool SaveExists(string key) {
        var path = savePath + key + saveFileExtension;
        return File.Exists(path);
    }
}