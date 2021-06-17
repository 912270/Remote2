package com.example.remote2client;

import android.graphics.BitmapFactory;
import android.util.Log;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.ObjectInputStream;
import java.net.Socket;

public class Connection {
    private static Socket mSocket = null;
    private static String mHost = null;
    private static int mPort = 0;

    public static final String LOG_TAG = "SOCKET";

    public Connection (final String host, final int port)
    {
        this.mHost = host;
        this.mPort = port;
    }

    public static void openConnection() throws Exception
    {
        closeConnection();
        try {
            mSocket = new Socket(mHost, mPort);
        } catch (IOException e) {
            throw new Exception("Невозможно создать сокет: "
                    + e.getMessage());
        }
    }

    public static void closeConnection()
    {
        if (mSocket != null && !mSocket.isClosed()) {
            try {
                mSocket.close();
            } catch (IOException e) {
                Log.e(LOG_TAG, "Ошибка при закрытии сокета :"
                        + e.getMessage());
            } finally {
                mSocket = null;
            }
        }
        mSocket = null;
    }

    public static void sendData(byte[] data) throws Exception {
        if (mSocket == null || mSocket.isClosed()) {
            throw new Exception("Ошибка отправки данных. " +
                    "Сокет не создан или закрыт");
        }
        try {
            mSocket.getOutputStream().write(data);
            mSocket.getOutputStream().flush();
        } catch (IOException e) {
            throw new Exception("Ошибка отправки данных : "
                    + e.getMessage());
        }
    }

    public static void getPic() throws Exception {

        byte[] inputByte = null;
        int length = 0;
        DataInputStream dis = null;
        FileOutputStream fos = null;
        Long timeName=System.currentTimeMillis();

        try {
            try {
                dis = new DataInputStream(mSocket.getInputStream());
                fos = new FileOutputStream(new File("E:\\"+timeName+".DCM"));
                inputByte = new byte[1024];
                //System.out.println («Начать получать данные ...»);
                while ((length = dis.read(inputByte, 0, inputByte.length)) > 0) {
                    System.out.println(length);
                    fos.write(inputByte, 0, length);
                    fos.flush();
                }
                //System.out.println («Полный прием»);
            } finally {
                if (fos != null)
                    fos.close();
                if (dis != null)
                    dis.close();
                /*if (mSocket != null)
                    mSocket.close();*/
            }
        } catch (Exception e) {
        }


        /*if (mSocket == null || mSocket.isClosed()) {
            throw new Exception("Ошибка приема данных. " +
                    "Сокет не создан или закрыт");
        }
        try {

            InputStreamReader IR = new InputStreamReader(mSocket.getInputStream());
            BufferedReader BR = new BufferedReader(IR);

        } catch (IOException e) {
            throw new Exception("Ошибка отправки данных : "
                    + e.getMessage());
        }*/
    }

   /* public static void GetData(byte[] data) throws Exception {
        String object;
        if (mSocket == null || mSocket.isClosed()) {
            throw new Exception("Ошибка приема данных. " +
                    "Сокет не создан или закрыт");
        }
        try {
            ObjectInputStream obIn = new ObjectInputStream(mSocket.getInputStream());
            while ((object = (String) obIn.readObject()) != null){
                if (object.equals("Send photo")){
                    FileOutputStream out = new FileOutputStream("test2.jpg");
                    DataInputStream in = new DataInputStream(mSocket.getInputStream());
                    byte[] bytes = new byte[5*1024];
                    int count, total=0;
                    long lenght = in.readLong();
                    while ((count = in.read(bytes)) > -1) {
                        total+=count;
                        out.write(bytes, 0, count);
                        if (total==lenght) break;
                    }
                    out.close();
                }
            }

            //mSocket.getOutputStream().write(data);
            //mSocket.getOutputStream().flush();
        } catch (IOException e) {
            throw new Exception("Ошибка отправки данных : "
                    + e.getMessage());
        }
    }*/
}
