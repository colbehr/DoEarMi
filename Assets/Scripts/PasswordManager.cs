using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;  
using System.Security.Cryptography;  
using UnityEngine;

public class PasswordManager
{
    // Singleton
    private static PasswordManager instance = null;
    private static readonly object instance_padlock = new object();

    private static int SALT_LENGTH = 32;

    // Instance is null initializer
    private PasswordManager()
    {
        // eh?
    }

    // Entry point for PasswordManager singleton
    public static PasswordManager Instance()
    {
        // double locking
        if (instance == null)
        {
            lock (instance_padlock)
            {
                if (instance == null)
                {
                    instance = new PasswordManager();
                }
            }
        }

        return instance;
    }


    // open source https://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp
    public static bool compare_byte_arrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
            {
            return false;
            }
        }

        return true;
    }


    // open source https://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp
    public string generate_salt_hash(string input, byte[] salt)
    {
        // Generate the hash
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations: 5000);
        return Convert.ToBase64String(pbkdf2.GetBytes(20));
    }


    public byte[] text_to_byte(string plainText)
    {
        return Encoding.UTF8.GetBytes(plainText);
    }


    // open source https://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp  
    public string create_salt()
    {
        //Generate a cryptographic random number.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[SALT_LENGTH];
        rng.GetBytes(buff);

        // Return a Base64 string representation of the random number.
        return Convert.ToBase64String(buff);
    }


    public bool is_correct_password(string pw_attempted, User user)
    {
        string user_hashed_pw = user.get_password_hash();
        string user_salt = user.get_password_salt();

        string attempt_hashed_pw = generate_salt_hash(pw_attempted, text_to_byte(user_salt));
        
        return compare_byte_arrays(text_to_byte(user_hashed_pw), text_to_byte(attempt_hashed_pw));
    }
}
