import cv2
import numpy as np
import pandas as pd
import sqlite3
import math

from matplotlib import pyplot as plt
from loremipsum import get_sentences
from sqlalchemy import create_engine
from IPython.display import display
from sklearn import linear_model
import glob

#print(glob.glob("C:/Users/victo/Desktop/Crowd Dataset/DataSet novo/*.txt"))

list_first_agent_aux = []
list_second_agent_aux = []
list_first_agent_pos_x_aux = []
list_first_agent_pos_y_aux = []
list_second_agent_pos_x_aux = []
list_second_agent_pos_y_aux = []
list_frame_aux = []
list_first_agent_o_aux = []
list_first_agent_c_aux = []
list_first_agent_e_aux = []
list_first_agent_a_aux = []
list_first_agent_n_aux = []
list_second_agent_o_aux = []
list_second_agent_c_aux = []
list_second_agent_e_aux = []
list_second_agent_a_aux = []
list_second_agent_n_aux = []
list_country_aux = []
list_video_aux = []
list_first_sec_vel = []
list_first_sec_dir = []
#print(maxFrame_person)
list_distance_first_second_agent = []

for xs in range(len(glob.glob("C:/Users/victo/Desktop/Crowd Dataset/DataSet novo/*.txt"))):
    
    testsplit = glob.glob("C:/Users/victo/Desktop/Crowd Dataset/DataSet novo/*.txt")[xs].split('.')
    testsplit = testsplit[0].split('_')
    testsplit2 = testsplit[1].split('-')
    #print(testsplit2)
    
    video_data = 'C:/Users/victo/Desktop/Crowd Dataset/DataSet novo/DATA_BR-01.txt'
    txtPaths = 'C:/Users/victo/Desktop/Crowd Dataset/CC-DataSet/'+testsplit2[0]+'/'+testsplit2[0]+'-'+testsplit2[1]+'/Paths_D.txt'
    
    peopleList = []
    pos_x_list = []
    pos_y_list = []
    frame_list = []
    countPerson = 1
    person = ""
    withoutTab = ""
    withoutParentheses = ""
    withoutTab2 = ""
    withoutcomma = ""
    testList = []

    withoutdotcomma = ""
    column_video_data_withoutdotcomma = ""
    columns_datas_withoutdotcomma = ""
    read_line_list = []
    list_country = []
    list_video = []
    list_frame_video = []
    list_agent_id = []
    list_Pos_X = []
    list_Pos_Y = []
    list_Pos_X_Perspectiva = []
    list_Pos_Y_Perspectiva = []
    list_active = []
    list_velocity = []
    list_direction = []
    list_dist_from_others = []
    list_O = []
    list_C = []
    list_E = []
    list_A = []
    list_N = []

    column_video_data = []
    with open(glob.glob("C:/Users/victo/Desktop/Crowd Dataset/DataSet novo/*.txt")[xs], "r") as video:
        for read_line in video:
            #print(read_line)
            read_line_list.append(read_line)
            #print(read_line_list[0])

    column_video_data_withoutdotcomma = read_line_list[0].split('\n')
    column_video_data_withoutdotcomma = column_video_data_withoutdotcomma[0].split(';')
    #print(column_video_data_withoutdotcomma)
    #print(read_line_list)
    for i in range(1, len(read_line_list)):
        #print(read_line_list[i])
        columns_datas_withoutdotcomma = read_line_list[i].split('\n')
        columns_datas_withoutdotcomma = columns_datas_withoutdotcomma[0].split(';')
        #print(columns_datas_withoutdotcomma)
        list_country.append(columns_datas_withoutdotcomma[0])
        list_video.append(columns_datas_withoutdotcomma[1])
        list_frame_video.append(int(columns_datas_withoutdotcomma[2]))
        list_agent_id.append(int(columns_datas_withoutdotcomma[3]))
        list_Pos_X.append(int(columns_datas_withoutdotcomma[4]))
        list_Pos_Y.append(int(columns_datas_withoutdotcomma[5]))
        #print(int(columns_datas_withoutdotcomma[5]))
        list_Pos_X_Perspectiva.append(int(columns_datas_withoutdotcomma[6]))
        list_Pos_Y_Perspectiva.append(int(columns_datas_withoutdotcomma[7]))
        list_active.append(columns_datas_withoutdotcomma[8])
        list_velocity.append(float(columns_datas_withoutdotcomma[9]))
        list_direction.append(float(columns_datas_withoutdotcomma[10]))
        list_dist_from_others.append(float(columns_datas_withoutdotcomma[11]))
        list_O.append(float(columns_datas_withoutdotcomma[12]))
        list_C.append(float(columns_datas_withoutdotcomma[13]))
        list_E.append(float(columns_datas_withoutdotcomma[14]))
        list_A.append(float(columns_datas_withoutdotcomma[15]))
        list_N.append(float(columns_datas_withoutdotcomma[16]))

    #d_video_data = {'Country': list_country, 'Video': list_video, 'Frame': list_frame_video, 'Agent_id': list_agent_id, 'Active': list_active, 'Velocity': list_velocity, 'Direction'}
    d_video_data = {column_video_data_withoutdotcomma[0]: list_country, column_video_data_withoutdotcomma[1]: list_video, column_video_data_withoutdotcomma[2]: list_frame_video, column_video_data_withoutdotcomma[3]: list_agent_id, column_video_data_withoutdotcomma[4]: list_Pos_X, column_video_data_withoutdotcomma[5]: list_Pos_Y, column_video_data_withoutdotcomma[6]: list_Pos_X_Perspectiva, column_video_data_withoutdotcomma[7]: list_Pos_Y_Perspectiva, column_video_data_withoutdotcomma[8]: list_active, column_video_data_withoutdotcomma[9]: list_velocity, column_video_data_withoutdotcomma[10]: list_direction, column_video_data_withoutdotcomma[11]: list_dist_from_others, column_video_data_withoutdotcomma[12]: list_O, column_video_data_withoutdotcomma[13]: list_C, column_video_data_withoutdotcomma[14]: list_E, column_video_data_withoutdotcomma[15]: list_A, column_video_data_withoutdotcomma[16]: list_N}
    df_video_data = pd.DataFrame(data=d_video_data)
    #print(df_video_data)
    #df_video_data_test = df_video_data[df_video_data['Agent_id'] == str(16)]
    #df_video_data_test2 = df_video_data_test[df_video_data_test['Active'] == "yes"]
    #print(df_video_data_test2)
    #print(df_video_data)

    with open(txtPaths,"r") as fi:
        for ln in fi:
            testList.append(ln)
    variableM = testList[0]
    variableM_withoutthings = variableM.split('[')
    variableM_withoutthings = variableM.split(']\n')
    variableM_withoutthings2 = variableM_withoutthings[0].split('[')
    variableM_list = []
    
    minFrame_data = df_video_data['Frame'].min()
    maxFrame_data = df_video_data['Frame'].max()
    minFrame_person = df_video_data['Agent_id'].min()
    maxFrame_person = df_video_data['Agent_id'].max()


    df_frame1 = df_video_data[df_video_data['Frame'] == 1]
    #df_frame1_br03 = df_frame1[df_frame1['Video'] == "UK-01"]
   #print(df_frame1_br03)
    counttest = 0
    for i in df_frame1.values:        
        agent1oceantest = (0.45 * i[10]) + (0.05 * (1-i[3])) + (0.45 * i[7]) + (0.05 * i[2]) + (0.05 * (1 - i[9]))
        #print(agent1oceantest)     
        counttest += agent1oceantest
    print(testsplit2[0]+'-'+testsplit2[1]+ " - " + str(counttest))
    for k in range(minFrame_data, maxFrame_data+1):
        df_frame = df_video_data[df_video_data['Frame'] == k]
        vel_max = df_frame['Velocity'].max()
        #print(df_frame)   
        #print("--------------------------")
        for i in df_frame.values:
            for j in df_frame.values:
                #print(i)
                #print(i[1] != j[1] and (math.sqrt((math.pow(j[11] - i[11], 2) + math.pow(j[13] - i[13], 2)))/float(variableM_withoutthings2[1])) <= 1.2 and (math.sqrt((math.pow(j[11] - i[11], 2) + math.pow(j[13] - i[13], 2)))/float(variableM_withoutthings2[1])) != 0 and (((j[15] - i[15])*100/vel_max) <= (vel_max*5)/100 and j[5] - i[5] <= 15))
                
                if i[1] != j[1] and (math.sqrt((math.pow(j[11] - i[11], 2) + math.pow(j[13] - i[13], 2)))/float(variableM_withoutthings2[1])) <= 1.2 and (math.sqrt((math.pow(j[11] - i[11], 2) + math.pow(j[13] - i[13], 2)))/float(variableM_withoutthings2[1])) != 0:
                    #and (((j[15] - i[15])*100)/vel_max) <= (vel_max*5)/100 and j[5] - i[5] <= 15 and (((j[15] - i[15])*100)/vel_max) != 0
                    agent1ocean = (0.45 * i[10]) + (0.05 * (1-i[3])) + (0.45 * i[7]) + (0.05 * i[2]) + (0.05 * (1 - i[9]))
                    agent2ocean = (0.45 * j[10]) + (0.05 * (1-j[3])) + (0.45 * j[7]) + (0.05 * j[2]) + (0.05 * (1 - j[9]))
                    if agent1ocean >= np.arange(0.1, 0.5) and agent2ocean >= np.arange(0.1, 0.5):
                        list_first_agent_aux.append("agent"+str(i[1]))
                        list_second_agent_aux.append("agent"+str(j[1]))
                        list_first_agent_pos_x_aux.append(i[11]/float(variableM_withoutthings2[1]))
                        list_first_agent_pos_y_aux.append(i[13]/float(variableM_withoutthings2[1]))
                        #if i[13] > 400:
                            #print("FODEU!!")
                        list_second_agent_pos_x_aux.append(j[11]/float(variableM_withoutthings2[1]))
                        list_second_agent_pos_y_aux.append(j[13]/float(variableM_withoutthings2[1]))
                        list_frame_aux.append(k)
                        list_first_agent_o_aux.append(i[10])
                        list_first_agent_c_aux.append(i[3])
                        list_first_agent_e_aux.append(i[7])
                        list_first_agent_a_aux.append(i[2])
                        list_first_agent_n_aux.append(i[9])
                        list_second_agent_o_aux.append(j[10])
                        list_second_agent_c_aux.append(j[3])
                        list_second_agent_e_aux.append(j[7])
                        list_second_agent_a_aux.append(j[2])
                        list_second_agent_n_aux.append(j[9])
                        list_country_aux.append(i[4])
                        list_video_aux.append(i[16])
                        

'''columns_aux = ['Frame', 'Country','Simulation','FirstAgent', 'FirstAgentPosX', 'FirstAgentPosY', 'FirstAgentO','FirstAgentC','FirstAgentE','FirstAgentA','FirstAgentN', 'SecondAgent', 'SecondAgentPosX', 'SecondAgentPosY', 'SecondAgentO', 'SecondAgentC', 'SecondAgentE', 'SecondAgentA', 'SecondAgentN']
d_data_aux = {'Frame': list_frame_aux, 'Country': list_country_aux, 'Simulation': list_video_aux,'FirstAgent': list_first_agent_aux, 'FirstAgentPosX': list_first_agent_pos_x_aux, 'FirstAgentPosY': list_first_agent_pos_y_aux, 'FirstAgentO': list_first_agent_o_aux,'FirstAgentC': list_first_agent_c_aux, 'FirstAgentE': list_first_agent_e_aux,'FirstAgentA': list_first_agent_a_aux,'FirstAgentN': list_first_agent_n_aux, 'SecondAgent': list_second_agent_aux, 'SecondAgentPosX': list_second_agent_pos_x_aux, 'SecondAgentPosY': list_second_agent_pos_y_aux, 'SecondAgentO': list_second_agent_o_aux, 'SecondAgentC': list_second_agent_c_aux, 'SecondAgentE': list_second_agent_e_aux, 'SecondAgentA': list_second_agent_a_aux, 'SecondAgentN': list_second_agent_n_aux}
df_data_aux = pd.DataFrame(data=d_data_aux)
list_two_persons = []
#print(df_data_aux)
#Create dataset db2
disk_engine_df_tot = create_engine('sqlite:///data_df_tot_file.db')
df_data_aux.to_sql('data_df_tot_file', disk_engine_df_tot)'''
'''print(df_data_aux)
for i in df_data_aux.values:
    list_two_persons.append((i[1], i[10]))
    #print(i)'''
    
