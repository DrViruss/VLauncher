package com.viruss.vlauncher;

import javafx.application.Application;

import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.CheckBox;
import javafx.scene.control.ComboBox;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class Main extends Application {


    @Override
    public void start(Stage stage) throws Exception{
        Parent root = FXMLLoader.load(getClass().getResource("sample.fxml"));
    //<editor-fold desc="XML to code">
        TextField passwordTF = (TextField) root.lookup("#passwordTF");
        TextField nameTF = (TextField) root.lookup("#nameTF");
        TextField dirTF = (TextField) root.lookup("#dirTF");

        CheckBox mojangChB = (CheckBox) root.lookup("#mojangChB");
        CheckBox javaChB = (CheckBox) root.lookup("#javaChB");
        CheckBox debugChB = (CheckBox) root.lookup("#debugChB");

        Button playBTN = (Button) root.lookup("#playBTN");
        Button dirBTN = (Button) root.lookup("#dirBTN");

        ComboBox packCB = (ComboBox) root.lookup("packCB");
//</editor-fold>

        passwordTF.setDisable(true);


    //<editor-fold desc="stage">
        stage.setTitle("Hello World");
        stage.setScene(new Scene(root, 600, 400));
        stage.show();
        //</editor-fold>

    }


    public static void main(String[] args) {
        launch(args);
    }
}
//Test area
/*    private final GridPane MainGP = new GridPane();
    private final GridPane LTopGP = new GridPane();
    private final GridPane RTopGP = new GridPane();
    private final GridPane LBotGP = new GridPane();
    private final TextField nameTF = new TextField("nickname");*/
    /*
    //<editor-fold desc="GridPanes">

    //<editor-fold desc="MainGP_Settings">
        MainGP.setGridLinesVisible(true);
        MainGP.setPrefSize(600,400);

        MainGP.row

        MainGP.add(nameTF,0,0);

        //<editor-fold desc="Cols">
            //0
            MainGP.getColumnConstraints().get(0).setMaxWidth(243);
            MainGP.getColumnConstraints().get(0).setMinWidth(10);
            MainGP.getColumnConstraints().get(0).setPrefWidth(236);
            //1
            MainGP.getColumnConstraints().get(1).setMaxWidth(194);
            MainGP.getColumnConstraints().get(1).setMinWidth(10);
            MainGP.getColumnConstraints().get(1).setPrefWidth(142);
            MainGP.getColumnConstraints().get(1).setHalignment(HPos.CENTER);
            //2
            MainGP.getColumnConstraints().get(2).setMaxWidth(237);
            MainGP.getColumnConstraints().get(2).setMinWidth(10);
            MainGP.getColumnConstraints().get(2).setPrefWidth(222);
        //</editor-fold>
        //<editor-fold desc="Rows">
            //0
            MainGP.getRowConstraints().get(0).setMaxHeight(145);
            MainGP.getRowConstraints().get(0).setMinHeight(10);
            MainGP.getRowConstraints().get(0).setPrefHeight(145);
             //1
            MainGP.getRowConstraints().get(1).setMaxHeight(127);
            MainGP.getRowConstraints().get(1).setMinHeight(10);
            MainGP.getRowConstraints().get(1).setPrefHeight(100);
            //2
            MainGP.getRowConstraints().get(2).setMaxHeight(155);
            MainGP.getRowConstraints().get(2).setMinHeight(10);
            MainGP.getRowConstraints().get(2).setPrefHeight(155);
//</editor-fold>



        /*
            (0, 0) 	(1, 0) 	(2, 0)
            (2, 1) 	(1, 1) 	(0, 1)
            (2, 2) 	(1, 2) 	(0, 2)

        gp.add(new TextField("00"),0,0);
        gp.add(new TextField("01"),0,1);
        gp.add(new TextField("02"),0,2);
        gp.add(new TextField("10"),1,0);
        gp.add(new TextField("11"),1,1);
        gp.add(new TextField("12"),1,2);
        gp.add(new TextField("20"),2,0);
        gp.add(new TextField("21"),2,1);
        gp.add(new TextField("22"),2,2);




//</editor-fold>

    //<editor-fold desc="TopLefGP_Settings">

        LTopGP.setGridLinesVisible(true);
        //<editor-fold desc="Cols">
        //0
        LTopGP.getColumnConstraints().get(0).setMaxWidth(143);
        LTopGP.getColumnConstraints().get(0).setMinWidth(10);
        LTopGP.getColumnConstraints().get(0).setPrefWidth(32);
        //1
        LTopGP.getColumnConstraints().get(1).setMaxWidth(288);
        LTopGP.getColumnConstraints().get(1).setMinWidth(10);
        LTopGP.getColumnConstraints().get(1).setPrefWidth(260);
        //2
        LTopGP.getColumnConstraints().get(2).setMaxWidth(288);
        LTopGP.getColumnConstraints().get(2).setMinWidth(10);
        LTopGP.getColumnConstraints().get(2).setPrefWidth(30);
        //</editor-fold>
        //<editor-fold desc="Rows">
        //0
        LTopGP.getRowConstraints().get(0).setMaxHeight(39);
        LTopGP.getRowConstraints().get(0).setMinHeight(8);
        LTopGP.getRowConstraints().get(0).setPrefHeight(14);
        //1
        LTopGP.getRowConstraints().get(1).setMaxHeight(82);
        LTopGP.getRowConstraints().get(1).setMinHeight(10);
        LTopGP.getRowConstraints().get(1).setPrefHeight(48);
        //2
        LTopGP.getRowConstraints().get(2).setMaxHeight(73);
        LTopGP.getRowConstraints().get(2).setMinHeight(10);
        LTopGP.getRowConstraints().get(2).setPrefHeight(67);
//</editor-fold>

//</editor-fold>

    //<editor-fold desc="TopRigGP_Settings">
        RTopGP.setGridLinesVisible(true);
        //<editor-fold desc="Cols">
        //0
        RTopGP.getColumnConstraints().get(0).setMaxWidth(266);
        RTopGP.getColumnConstraints().get(0).setMinWidth(10);
        RTopGP.getColumnConstraints().get(0).setPrefWidth(22);
        //1
        RTopGP.getColumnConstraints().get(1).setMaxWidth(268);
        RTopGP.getColumnConstraints().get(1).setMinWidth(10);
        RTopGP.getColumnConstraints().get(1).setPrefWidth(268);
        //2
        RTopGP.getColumnConstraints().get(2).setMaxWidth(283);
        RTopGP.getColumnConstraints().get(2).setMinWidth(10);
        RTopGP.getColumnConstraints().get(2).setPrefWidth(34);
        //</editor-fold>
        //<editor-fold desc="Rows">
        //0
        RTopGP.getRowConstraints().get(0).setMinHeight(10);
        RTopGP.getRowConstraints().get(0).setPrefHeight(30);
        //1
        RTopGP.getRowConstraints().get(1).setMinHeight(10);
        RTopGP.getRowConstraints().get(1).setPrefHeight(30);
        //2
        RTopGP.getRowConstraints().get(2).setMinHeight(10);
        RTopGP.getRowConstraints().get(2).setPrefHeight(30);
//</editor-fold>
//</editor-fold>

    //<editor-fold desc="BotLefGP_Settings">
        LBotGP.setGridLinesVisible(true);
        //<editor-fold desc="Cols">
        //0
        LBotGP.getColumnConstraints().get(0).setMaxWidth(72);
        LBotGP.getColumnConstraints().get(0).setMinWidth(10);
        LBotGP.getColumnConstraints().get(0).setPrefWidth(26);
        //1
        LBotGP.getColumnConstraints().get(1).setMaxWidth(180);
        LBotGP.getColumnConstraints().get(1).setMinWidth(10);
        LBotGP.getColumnConstraints().get(1).setPrefWidth(180);
        LBotGP.getColumnConstraints().get(1).setHalignment(HPos.CENTER);
        //2
        LBotGP.getColumnConstraints().get(2).setMaxWidth(86);
        LBotGP.getColumnConstraints().get(2).setMinWidth(10);
        LBotGP.getColumnConstraints().get(2).setPrefWidth(32);
        //</editor-fold>
        //<editor-fold desc="Rows">
        //0
        LBotGP.getRowConstraints().get(0).setMinHeight(10);
        LBotGP.getRowConstraints().get(0).setPrefHeight(30);
        //1
        LBotGP.getRowConstraints().get(1).setMinHeight(10);
        LBotGP.getRowConstraints().get(1).setPrefHeight(30);
        //2
        LBotGP.getRowConstraints().get(2).setMinHeight(10);
        LBotGP.getRowConstraints().get(2).setPrefHeight(30);
//</editor-fold>
//</editor-fold>

//</editor-fold>

    //<editor-fold desc="Controls">
        nameTF.setPrefSize(100,5);
        //</editor-fold>

    //<editor-fold desc="Stage/Scene">
        stage.setTitle("VLauncher");
        stage.setWidth(300);
        stage.setHeight(300);
        Scene scene = new Scene(MainGP);
        stage.setScene(scene);
        stage.show();
//</editor-fold>
*/