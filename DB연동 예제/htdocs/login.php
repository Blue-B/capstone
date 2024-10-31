<?php
$host = 'localhost';
$user = 'root';
$pw = '';
$database = 'unity';

try {
    // 데이터베이스 연결
    $dbhandle = new PDO('mysql:host=' . $host . ';dbname=' . $database, $user, $pw);
    $dbhandle->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    echo json_encode(['error' => $e->getMessage()]);
    exit();
}

// POST로 받은 userid와 userpassword 처리
$userid = $_POST['email'];   // 유니티에서 이메일 필드에 입력한 값을 userid로 받음
$password = $_POST['password'];

try {
    // userid와 userpassword를 확인하는 SQL 쿼리
    $stmt = $dbhandle->prepare('SELECT * FROM users WHERE userid = :userid AND userpassword = :password');
    $stmt->bindParam(':userid', $userid, PDO::PARAM_STR);
    $stmt->bindParam(':password', $password, PDO::PARAM_STR);
    $stmt->execute();

    // 결과 가져오기
    $user = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($user) {
        // 로그인 성공 시 "success" 메시지 반환
        echo json_encode(['status' => 'success', 'userid' => $user['userid']]);
    } else {
        // 로그인 실패 시 오류 메시지 반환
        echo json_encode(['status' => 'error', 'message' => 'Invalid userid or password']);
    }
} catch (Exception $e) {
    echo json_encode(['error' => $e->getMessage()]);
}
?>
