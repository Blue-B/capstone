<?php
$host = 'localhost';
$user = 'root';
$pw = ''; 
$database = 'unity'; /* 데이터베이스 이름 */

try {
    /* 데이터베이스에 연결 */
    $dbhandle = new PDO('mysql:host='.$host.';dbname='.$database, $user, $pw);
    $dbhandle->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
}
catch (PDOException $e) {
    echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(), '</pre>';
    exit();
}

// POST로 받은 데이터 처리
$userid = $_POST['userid'];
$userpassword = $_POST['userpassword'];
$name = $_POST['name'];
$birth_date = $_POST['birth_date'];
$phone_number = $_POST['phone_number'];

try {
    // SQL 명령문 준비 - users 테이블에 데이터를 삽입
    $shandle = $dbhandle->prepare('INSERT INTO users (userid, userpassword, name, birth_date, phone_number) VALUES (:userid, :userpassword, :name, :birth_date, :phone_number)');

    // 파라미터 바인딩
    $shandle->bindParam(':userid', $userid, PDO::PARAM_STR);
    $shandle->bindParam(':userpassword', $userpassword, PDO::PARAM_STR);
    $shandle->bindParam(':name', $name, PDO::PARAM_STR);
    $shandle->bindParam(':birth_date', $birth_date, PDO::PARAM_STR);
    $shandle->bindParam(':phone_number', $phone_number, PDO::PARAM_STR);

    // 명령문 실행
    $shandle->execute();
    echo "Data successfully inserted.";
}
catch (PDOException $e) {
    // 중복된 userid가 삽입될 경우, MySQL 고유 제약 조건 위반으로 오류가 발생할 수 있음
    if ($e->getCode() == 23000) { // 23000은 MySQL의 UNIQUE 제약 조건 위반 코드
        echo "Error: User ID already exists!";
    } else {
        echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(), '</pre>';
    }
}
?>
