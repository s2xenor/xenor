<?php
include('./config.php');

/*
* User1
* User2
* Score
*/

if(isset($_GET["u1"]) && isset($_GET["u2"]) && isset($_GET["score"]) && !empty($_GET["u1"]) && !empty($_GET["u2"]) && !empty($_GET["score"])){
    $u1 = htmlspecialchars($_GET["u1"]);
    $u2 = htmlspecialchars($_GET["u2"]);
    $score = htmlspecialchars($_GET["score"]);

    $addScore = $bdd->prepare("INSERT INTO scores (user1, user2, score) VALUES (?, ?, ?)");
    $addScore->execute(array($u1, $u2, $score));
    header("HTTP/1.1 200 OK");
}else{
    header("HTTP/1.1 400 Bad Request");
}

?>