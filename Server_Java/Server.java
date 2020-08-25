package recom.tensorflow.tutorial;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.EOFException;
import java.io.IOException;
import java.net.*;

import org.tensorflow.SavedModelBundle;
import org.tensorflow.Session;
import org.tensorflow.*;
import org.tensorflow.Tensor;
import org.tensorflow.TensorFlow;

class need {
	public static int vx;
	public static int vy;
	public static float[][] dataset;
}

public class HelloTF {
	public static void main(String[] args) throws IOException {
		System.out.println("TensorFlow version : " + TensorFlow.version());

		try {
			@SuppressWarnings("resource")
			ServerSocket serversocket = new ServerSocket(9003);

			System.out.println("Client의 접속을 기다리고 있습니다..");

			Socket socket = serversocket.accept();

			new readwrite_S(socket).start();

		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}

class KalmanFilter {
	private double Q = 0.00001;
	private double R = 0.001;
	private double X = 0, P = 1, K;

	// 생성자에는 초기값을 넣어주어야 한다. 이전에 수치가 없으면 아무 의미가 없으므로
	KalmanFilter(double initValue) {
		X = initValue;
	}

	// 현재값을 받아 계산된 공식을 적용하고 반환한다
	public double update(double measurement) {
		measurementUpdate();

		X = X + (measurement - X) * K;

		return X;
	}

	// 이전의 값들을 공식을 이용하여 계산한다.
	private void measurementUpdate() {
		K = (P + Q) / (P + Q + R);
		P = R * (P + Q) / (P + Q + R);
	}
}

class readwrite_S extends Thread {
	public readwrite_S(Socket socket) {
		this.socket = socket;
	}

	Socket socket;
	int ordering = 0;
	int c1 = 0 ;
	int c2 = 1 ;

	String[] bcName = { "50:51:A9:7B:33:CD", "F8:30:02:25:14:8C", "50:51:A9:7B:05:F5", "F8:30:02:29:74:5C" };
	String[] signal = new String[4];
	int[] kalmanRSSI = new int[4];

	double k = -80;

	KalmanFilter kf1 = new KalmanFilter(k);
	KalmanFilter kf2 = new KalmanFilter(k);
	KalmanFilter kf3 = new KalmanFilter(k);
	KalmanFilter kf4 = new KalmanFilter(k);

	@Override
	public void run()
	{
		need.dataset = new float[1][4] ;
		
		System.out.println("Im ready");
		
		DataInputStream dis = null;   
		DataOutputStream dos = null;
		
		try
		{
			dis = new DataInputStream(socket.getInputStream());
			dos = new DataOutputStream(socket.getOutputStream());

			while (true)
			{
				try
				{
					String readmsg = dis.readUTF();
					
					if (readmsg.contains(bcName[ordering]) == true)
					{
						signal[ordering] = readmsg.substring(19);
						
						if( Integer.parseInt(signal[ordering]) >= 0 )
						{
						}else
						{
							kalmanRSSI[ordering] = Integer.parseInt(signal[ordering]);
							
			                if (ordering == 0)
			                {
			                	kalmanRSSI[ordering] = (int) kf1.update(kalmanRSSI[ordering]);
			                } else if (ordering == 1)
			                {
			                   kalmanRSSI[ordering] = (int) kf2.update(kalmanRSSI[ordering]);
			                } else if (ordering == 2)
			                {
			                   kalmanRSSI[ordering] = (int) kf3.update(kalmanRSSI[ordering]);
			                } else if (ordering == 3)
			                {
			                   kalmanRSSI[ordering] = (int) kf4.update(kalmanRSSI[ordering]);
			                   c1 = 1 ;
			                }

			                if (ordering < 3) ordering++;
			                else ordering = 0;
			                
//			                if( ordering == 3 )
//			                {
//			                	System.out.println("[System.out] " + String.valueOf(ordering) + " " + String.valueOf(signal[ordering])) ;
//			                }else
//			                {
//			                	System.out.println(String.valueOf(ordering) + " " + String.valueOf(signal[ordering])) ;
//			                }
						}
		            }

		            if ( c1 == c2 )
		            {
		            	for (int i = 0; i < 4; i++)
		                {
		            		need.dataset[0][i] = kalmanRSSI[i] ;
		                }
		            	 	
		            	SavedModelBundle b = SavedModelBundle.load("/tempfinal/fromPython", "serve") ;
			            Session sess = b.session();		            
			            Tensor x = Tensor.create(need.dataset);
			            float[][] y = sess.runner().feed("X", x).fetch("h").run().get(0).copyTo(new float[1][2]);
			            sess.close();
			            
			            need.vx = (int) y[0][0];
				        need.vy = (int) y[0][1];
		            	
		            	c1 = 2 ;
		            }
		        } catch (EOFException eof){
		        	eof.printStackTrace();
		        }
				
				if (c1 == 2)
				{
					String writemsg = String.valueOf(need.vx) + " " + String.valueOf(need.vy);
					System.out.println("[System.out] " + String.valueOf(need.dataset[0][0]) + " " + String.valueOf(need.dataset[0][1]) + " " + String.valueOf(need.dataset[0][2]) + " " + String.valueOf(need.dataset[0][3]));
			    	System.out.println("[System.out] " + String.valueOf(need.vx) + " " + String.valueOf(need.vy)) ;
			    	dos.writeUTF(writemsg);
					
					c1 = 0 ;
				}
			}
		}catch(Exception e)
		{
			e.getStackTrace();
		}finally
		{ 
			try
			{
				dis.close();
				dos.close();
			}catch (IOException e)
			{
				//e.printStackTrace();
			}
		}
	}
}