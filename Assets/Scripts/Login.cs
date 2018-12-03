using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    public Button enterButton;
    public GameObject username, password;
    public Text usernameError, passwordError;
    private string Username, Password;
    string CreateUserURL = "kunet.kingston.ac.uk/k1628052/Login.php";

    // Use this for initialization
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        //TAB Cycling through Fields
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
  
        }

        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        //print(Username);

        //Use Enter for the Enter Button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(UserLogin(Username, Password));
        }

    }

    public void ButtonLogin()
    {
        StartCoroutine(UserLogin(Username, Password));
    }

     private IEnumerator UserLogin(string username, string password)
    {
        usernameError.enabled = false;
        passwordError.enabled = false;

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW www = new WWW(CreateUserURL, form);
        yield return www;
        print(www.text.ToString().Contains("found"));

        if (!string.IsNullOrEmpty(www.error)) print(www.text.ToString());
        else
        {
            if (www.text.ToString().Contains("User") == true)
            {

                usernameError.enabled = true;
            }
            else if (www.text.ToString().Contains("Password") == true)
            {
                passwordError.enabled = true;
            }
            else
            {
                PhotonNetwork.player.NickName = username;
                SceneManager.LoadScene(1);
            }
        }



    }
}
