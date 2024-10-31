using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //유니티 scens  제어 관련 네임스페이스


public class ChangeScenes : MonoBehaviour
{
    public void ChangeSceneBtn()
    {
        switch(this.gameObject.name)
        {
            case "register_button":
            SceneManager.LoadScene("register");
            break;
            //register cancle
            case "cancle_button":
            SceneManager.LoadScene("sing in");
            break;

        }
    }
}
