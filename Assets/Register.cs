using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine;

public class Register : MonoBehaviour
{
    public GameObject username, email, password, rePassword;
    public Text emailError, usernameError, passwordError,emptyFields;
    public Button enterButton;
    private string Username, Email, Password, RePassword;
    private bool validEmail = false;
    string CreateUserURL = "kunet.kingston.ac.uk/k1628052/Register.php";

    // Use this for initialization
    void Start()
    {

        emailError.enabled = false;
        usernameError.enabled = false;
        passwordError.enabled = false;
        emptyFields.enabled = false;



    }

    // Update is called once per frame
    void Update()
    {
        
        //TAB Cycling through Fields
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                rePassword.GetComponent<InputField>().Select();
            }
            if (rePassword.GetComponent<InputField>().isFocused)
            {
                email.GetComponent<InputField>().Select();
            }
        }

        Username = username.GetComponent<InputField>().text;
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        RePassword = rePassword.GetComponent<InputField>().text;
        //print(Username);

        //Use Enter for the Enter Button
        if(Input.GetKeyDown(KeyCode.Return))
        {
            RegisterButton();
        }

    }

    public void RegisterButton()
    {
        emailError.enabled = false;
        usernameError.enabled = false;
        passwordError.enabled = false;
        emptyFields.enabled = false;
        //Check all the fields are filled up


        if (Username != "" && Email != "" && Password != "" && RePassword != "")
        {

            //Check if the Passwords Match

            if (Password == RePassword)
            {
                UserCreation(Username, Password, Email);

            }
            else passwordError.enabled = true;
            
        }
        else emptyFields.enabled = true;
        
    }

    public void UserCreation(string username, string password, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", email);

 
        WWW www = new WWW(CreateUserURL, form);
    }


}
