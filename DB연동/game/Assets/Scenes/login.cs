using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//로그인
public class login : MonoBehaviour
{
    public string loginURL = "https://ed31-123-215-51-252.ngrok-free.app/unity/login.php"; // PHP 파일 경로

    // InputField 및 Button 연결
    public TMP_InputField email_input;        // 이메일 입력 필드
    public TMP_InputField password_input;     // 비밀번호 입력 필드
    public Button login_button;               // 로그인 버튼

    void Start()
    {
        login_button.onClick.AddListener(OnLoginButtonClicked);  // 버튼 클릭 이벤트 추가
    }

    public void OnLoginButtonClicked()  
    {
        // 입력 필드에서 값 가져오기
        string email = email_input.text;
        string password = password_input.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("이메일 또는 비밀번호가 입력되지 않았습니다.");
            return;
        }

        // 코루틴으로 로그인 처리
        StartCoroutine(Login(email, password));
    }

    IEnumerator Login(string email, string password)
    {
        // POST 요청을 위한 WWWForm 생성
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // 서버에 로그인 요청 보내기
        UnityWebRequest request = UnityWebRequest.Post(loginURL, form);

        // 응답 대기
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 서버 응답 확인
            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);

            // 서버 응답을 LoginResponse 클래스로 파싱
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(responseText);

            if (loginResponse.status == "success")
            {
                // 로그인 성공 시 userID 저장
                PlayerPrefs.SetString("userID", loginResponse.userid);

                // Mypage 씬으로 이동
                SceneManager.LoadScene("Mypage");
            }
            else
            {
                Debug.LogError("못된 이메일 또는 비밀번호입니다.");
            }
        }
    }
}

// 서버 응답을 처리하기 위한 LoginResponse 클래스
[System.Serializable]
public class LoginResponse
{
    public string status;  // 로그인 성공 여부 (success)
    public string userid;  // 유저 ID
}
