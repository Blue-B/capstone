<?php
$host = 'localhost';
$user = 'root';
$pw = '';
$database = 'unity';

try {
    $dbhandle = new PDO('mysql:host=' . $host . ';dbname=' . $database, $user, $pw);
    $dbhandle->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    echo json_encode(['error' => $e->getMessage()]);
    exit();
}

// 입력받은 userid로 데이터 검색
$userid = $_GET['userid'];

try {
    $stmt = $dbhandle->prepare('SELECT name, birth_date, phone_number FROM users WHERE userid = :userid');
    $stmt->bindParam(':userid', $userid, PDO::PARAM_STR);
    $stmt->execute();
    
    // 결과를 배열 형태로 반환
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    
    if ($result) {
        echo json_encode($result);
    } else {
        echo json_encode(['error' => 'User not found']);
    }
} catch (Exception $e) {
    echo json_encode(['error' => $e->getMessage()]);
}
?>
