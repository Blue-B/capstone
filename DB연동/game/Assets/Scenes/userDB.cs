using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;  // 씬 변경을 위한 네임스페이스 추가
//회원가입
public class userDB : MonoBehaviour
{
 
    public string saveDataURL = "https://ed31-123-215-51-252.ngrok-free.app/unity/savedata.php";

    // UI 입력 필드와 버튼 연결
    public TMP_InputField user_id_input;       // 유저 ID 입력 필드
    public TMP_InputField user_password_input; // 유저 비밀번호 입력 필드
    public TMP_InputField name_input;          // 이름 입력 필드
    public TMP_InputField birth_date_input;    // 생년월일 입력 필드
    public TMP_InputField phone_number_input;  // 전화번호 입력 필드
    public Button register_button;         // 등록 버튼

    void Start() //중복실행방지
    {
    // 등록 이벤트가 중복으로 발생하지 않도록 ClearListeners를 사용
    register_button.onClick.RemoveAllListeners();  // 기존에 등록된 리스너 제거
    register_button.onClick.AddListener(Save);     // 버튼 클릭 이벤트 추가
    }


    public void Save()
    {
        //중복실행방지
        register_button.interactable = false;

        // 입력 필드에서 값 가져오기
        string userID = user_id_input.text;
        string userPassword = user_password_input.text;
        string name = name_input.text;
        string birthDate = birth_date_input.text;
        string phoneNumber = phone_number_input.text;

        // 코루틴 실행하여 데이터 전송
        StartCoroutine(PostData(userID, userPassword, name, birthDate, phoneNumber));
    }

    IEnumerator PostData(string userID, string userPassword, string name, string birthDate, string phoneNumber)
    {
        // POST 요청을 위한 WWWForm 생성
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("userpassword", userPassword);
        form.AddField("name", name);
        form.AddField("birth_date", birthDate);
        form.AddField("phone_number", phoneNumber);

        // 데이터 전송 URL 설정
        UnityWebRequest hs_post = UnityWebRequest.Post(saveDataURL, form);

        // 응답 대기
        yield return hs_post.SendWebRequest();

        if (hs_post.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("오류가 발생했습니다: " + hs_post.error);
        }
        else
        {
            Debug.Log("데이터베이스 전송완료");

            // 데이터 전송이 성공적으로 완료되면 mypage 씬으로 이동
            SceneManager.LoadScene("sing in");  // 씬 변경 추후 main으로 수정해야함
        }
        // 작업 완료 후 버튼 다시 활성화
        register_button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
