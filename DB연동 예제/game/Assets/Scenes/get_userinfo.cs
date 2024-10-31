using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
//마이페이지
public class get_userinfo : MonoBehaviour
{
    public string getDataURL = "https://ed31-123-215-51-252.ngrok-free.app/unity/getUserinfo.php"; // PHP 파일 경로

    // TextMeshPro 텍스트 필드
    public TMP_Text name_text;           // 이름 표시용 텍스트 필드
    public TMP_Text birth_date_text;     // 생년월일 표시용 텍스트 필드
    public TMP_Text phone_number_text;   // 전화번호 표시용 텍스트 필드

    void Start()
    {
        // 로그인 후 저장된 유저 ID를 불러옴
        string userID = PlayerPrefs.GetString("userID", "");

        // userID가 존재하는지 확인
        if (!string.IsNullOrEmpty(userID))
        {
            StartCoroutine(GetUserData(userID));
        }
        else
        {
            Debug.LogError("사용자 ID가 없습니다. 사용자 데이터를 로드할 수 없습니다.");
        }
    }

    IEnumerator GetUserData(string userID)
    {
        // URL 인코딩을 통해 userid의 특수문자를 처리
        string encodedUserID = UnityWebRequest.EscapeURL(userID);
        string url = getDataURL + "?userid=" + encodedUserID;
        Debug.Log("Request URL: " + url);  // URL이 제대로 생성되었는지 확인

        UnityWebRequest request = UnityWebRequest.Get(url);

        // 응답 대기
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching data: " + request.error);  // 오류 메시지 출력
        }
        else
        {
            // 서버 응답 확인
            string jsonData = request.downloadHandler.text;
            Debug.Log("Response received: " + jsonData);

            // JSON 데이터를 파싱
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);

            if (userData != null)
            {
                // 받아온 데이터를 TMP_Text 필드에 표시
                name_text.text = "Name: " + userData.name;
                birth_date_text.text = "Birth Date: " + userData.birth_date;
                phone_number_text.text = "Phone: " + userData.phone_number;
            }
            else
            {
                Debug.LogError("잘못된 사용자 데이터 응답");
            }
        }
    }
}

// 서버에서 받아올 데이터 구조체
[System.Serializable]
public class UserData
{
    public string name;
    public string birth_date;
    public string phone_number;
}
