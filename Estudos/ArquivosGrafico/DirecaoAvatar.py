import pandas as pd
import numpy as np
import csv
import os

import plotly     
import plotly.plotly as py
import plotly.graph_objs as go

contents = []
df = pd.read_csv("RespostasQuestionario.csv", encoding = "ISO-8859-1", engine='python', header = None, thousands=',')
#df = df.str.split()
#for i in df:
read_line_list = []
with open("RespostasQuestionario.csv", "r") as video:
        for read_line in video:
            #print(read_line)
            read_line_list.append(read_line)
#print(read_line_list)
column_video_data_withoutdotcomma = read_line_list[0].split('\n')
column_video_data_withoutdotcomma = column_video_data_withoutdotcomma[0].split(';')
#print(column_video_data_withoutdotcomma)
idadelista = []
escolaridadelista = []
generolista = []
baixadensidade = []
mediadensidade = []
altadensidade = []
camperspectdens = []
avatperspectiva = []
variavelo = []
camperspectvariavel = []
avaperspectvariavel = []
maiordistentrepess = []
campersdist = []
avapersdist = []
andamaisrap = []
muddedir = []
cammuddedir = []
avatmudandedir = []

for i in range(1, len(read_line_list)):
        #print(read_line_list[i])
        columns_datas_withoutdotcomma = read_line_list[i].split('\n')
        columns_datas_withoutdotcomma = columns_datas_withoutdotcomma[0].split(';')
        print(columns_datas_withoutdotcomma)
        idadelista.append(columns_datas_withoutdotcomma[0])
        escolaridadelista.append(columns_datas_withoutdotcomma[1])
        generolista.append(columns_datas_withoutdotcomma[2])
        baixadensidade.append(columns_datas_withoutdotcomma[3])
        mediadensidade.append(columns_datas_withoutdotcomma[4])
        altadensidade.append(columns_datas_withoutdotcomma[5])
        #print(int(columns_datas_withoutdotcomma[5]))
        camperspectdens.append(columns_datas_withoutdotcomma[6])
        avatperspectiva.append(columns_datas_withoutdotcomma[7])
        variavelo.append(columns_datas_withoutdotcomma[8])
        camperspectvariavel.append(columns_datas_withoutdotcomma[9])
        avaperspectvariavel.append(columns_datas_withoutdotcomma[10])
        maiordistentrepess.append(columns_datas_withoutdotcomma[11])
        campersdist.append(columns_datas_withoutdotcomma[12])
        avapersdist.append(columns_datas_withoutdotcomma[13])
        andamaisrap.append(columns_datas_withoutdotcomma[14])
        muddedir.append(columns_datas_withoutdotcomma[15])
        cammuddedir.append(columns_datas_withoutdotcomma[16])
        avatmudandedir.append(columns_datas_withoutdotcomma[17])

d_video_data = {column_video_data_withoutdotcomma[0]: idadelista, column_video_data_withoutdotcomma[1]: escolaridadelista, column_video_data_withoutdotcomma[2]: generolista, column_video_data_withoutdotcomma[3]: baixadensidade, column_video_data_withoutdotcomma[4]: mediadensidade, column_video_data_withoutdotcomma[5]: altadensidade, column_video_data_withoutdotcomma[6]: camperspectdens, column_video_data_withoutdotcomma[7]: avatperspectiva, column_video_data_withoutdotcomma[8]: variavelo, column_video_data_withoutdotcomma[9]: camperspectvariavel, column_video_data_withoutdotcomma[10]: avaperspectvariavel, column_video_data_withoutdotcomma[11]: maiordistentrepess, column_video_data_withoutdotcomma[12]: campersdist, column_video_data_withoutdotcomma[13]: avapersdist, column_video_data_withoutdotcomma[14]: andamaisrap, column_video_data_withoutdotcomma[15]: muddedir, column_video_data_withoutdotcomma[16]: cammuddedir, column_video_data_withoutdotcomma[17]: avatmudandedir}
df_video_data = pd.DataFrame(data=d_video_data)
print(df_video_data)

#layout = dict(sliders=sliders)

#fig = dict(data=slideTest, layout=layout)
avatar = df_video_data['AvatarPercepDensidade'].unique()
print(avatar)
countavatarPerspectiveDens = len(df_video_data[df_video_data['AvatarPercepDensidade'] == avatar[0]])
countavatarEgocentricDens = len(df_video_data[df_video_data['AvatarPercepDensidade'] == avatar[1]])
countavatarNHouveDens = len(df_video_data[df_video_data['AvatarPercepDensidade'] == avatar[2]])
trace = go.Pie(labels=avatar, values=[countavatarPerspectiveDens, countavatarEgocentricDens, countavatarNHouveDens])

countavatarPerspectiveVariaVelo = len(df_video_data[df_video_data['Avatar PercepVariaVelo'] == avatar[0]])
countavatarEgocentricVariaVelo = len(df_video_data[df_video_data['Avatar PercepVariaVelo'] == avatar[1]])
countavatarNHouveVariaVelo = len(df_video_data[df_video_data['Avatar PercepVariaVelo'] == avatar[2]])
trace2 = go.Pie(labels=avatar, values=[countavatarPerspectiveVariaVelo, countavatarEgocentricVariaVelo, countavatarNHouveVariaVelo])

countavatarPerspectiveDist = len(df_video_data[df_video_data['AvatPercepDistan'] == avatar[0]])
countavatarEgocentricDist = len(df_video_data[df_video_data['AvatPercepDistan'] == avatar[1]])
countavatarNHouveDist = len(df_video_data[df_video_data['AvatPercepDistan'] == avatar[2]])
trace3 = go.Pie(labels=avatar, values=[countavatarPerspectiveDist, countavatarEgocentricDist, countavatarNHouveDist])

countavatarPerspectiveDir = len(df_video_data[df_video_data['AvatMudanDedir'] == avatar[0]])
countavatarEgocentricDir = len(df_video_data[df_video_data['AvatMudanDedir'] == avatar[1]])
countavatarNHouveDir = len(df_video_data[df_video_data['AvatMudanDedir'] == avatar[2]])
trace4 = go.Pie(labels=avatar, values=[countavatarPerspectiveDir, countavatarEgocentricDir, countavatarNHouveDir])
#py.iplot([trace], filename='basic_pie_chart')

#fig = go.Figure(data=data, layout=layout)

fig = {
  "data": [
    {
      "values": [countavatarPerspectiveDir, countavatarEgocentricDir, countavatarNHouveDir],
      "labels": ["Both", avatar[1], avatar[2]],
      "text":[""],
      "textposition":"inside",
      "domain": {'x': [0.1, 1]},
      "name": "Perception of Change Direction",
      "hoverinfo":"label+percent+name",
      "hole": .4,
      "type": "pie"
    }],
  "layout": {
        "title":"Avatars",
        "annotations": [
            {
                "font": {
                    "size": 40
                },
                "showarrow": False,
                "text": "C.Direction",
                "x": 0.55,
                "y": 0.5
            }
        ]
    }
}
#py.iplot(fig, filename='donut')

#plotly.offline.plot([trace, trace2], filename='basic_pie_chart')
plotly.offline.plot(fig, filename='pie_chart')

'''with open("pesquisaanimacaocompt.txt", 'rb') as f:
  contents = f.read()
  print(contents)

 #contents = open("pesquisaanimacaocompt.txt").read()
with open('pesquisaanimacaocompt.txt', newline='') as csvfile:
     spamreader = csv.reader(csvfile, delimiter=' ', quotechar='|')
     for row in spamreader:
         print(', '.join(row))''' 
#testBigFile = 'pesquisaanimacaocompt.txt'
#testBigFilePD = pd.read_csv("pesquisaanimacaocompt.txt")
'testBigFilePDdf = pd.DataFrame(testBigFilePD)'
#print(testBigFile)

'''simulations = testBigFilePDdf['Simulation'].unique()
frame = testBigFilePDdf['Frame'].unique()

firstAgent = testBigFilePDdf['FirstAgent'].unique()
secondAgent = testBigFilePDdf['SecondAgent'].unique()
secondAgentO = testBigFilePDdf['SecondAgentO'].unique()

secondAgentC = testBigFilePDdf['SecondAgentC'].unique()
secondAgentE = testBigFilePDdf['SecondAgentE'].unique()
secondAgentA = testBigFilePDdf['SecondAgentA'].unique()
secondAgentN = testBigFilePDdf['SecondAgentN'].unique()
firstAgentO = testBigFilePDdf['FirstAgentO'].unique()
firstAgentC = testBigFilePDdf['FirstAgentC'].unique()
firstAgentE = testBigFilePDdf['FirstAgentE'].unique()
firstAgentA = testBigFilePDdf['FirstAgentA'].unique()
firstAgentN = testBigFilePDdf['FirstAgentN'].unique()'''
'''
N = 1000000
for i in testBigFilePDdf['Simulation'].unique():
    trace = go.Scattergl(
        x = testBigFilePDdf[testBigFilePDdf['Simulation'] == i]['FirstAgent'],
        y = testBigFilePDdf[testBigFilePDdf['Simulation'] == i]['SecondAgent'],
        mode = 'markers',
        marker = dict(
            color = 'rgb(152, 0, 0)',
            line = dict(
                width = 1,
                color = 'rgb(0,0,0)')
        )
    )
data = [trace]
#py.iplot(data, filename='Simulation/FirstAgent')
plotly.offline.plot(data, filename='Simulation-FirstAgent')

'''
'''data = []
#trace_num = 10
#point_num = 5000
for i in firstAgent:
    data.append(go.Scattergl(
        x = testBigFilePDdf[testBigFilePDdf['Simulation'] == i]['FirstAgent'],
        y = testBigFilePDdf[testBigFilePDdf['Simulation'] == i]['SecondAgent']    
        )
)
layout = dict(showlegend=False)
fig=dict(data=data, layout=layout)
plotly.offline.plot(fig, filename='Simulation-FirstAgent')'''