#import cv2
import numpy as np
import sqlite3
import math

#from matplotlib import pyplot as plt
from loremipsum import get_sentences
from sqlalchemy import create_engine
from IPython.display import display
#from sklearn import linear_model

import dash
from dash.dependencies import Input, Output
import dash_core_components as dcc
import dash_html_components as html
import pandas as pd
import plotly.graph_objs as go
import plotly.tools as tls
import datetime as dt
import networkx as nx
from decimal import Decimal

app = dash.Dash()
app.scripts.config.serve_locally = True
vertical = True

'''conTest = sqlite3.connect('bigfile.db')
testCon = conTest.cursor()'''

con_db = sqlite3.connect("data_df_tot_file.db")
cur_db = con_db.cursor()

test1 = pd.read_sql_query('SELECT * FROM data_df_tot_file', con_db)
countSimulations = pd.read_sql_query('SELECT Simulation, FirstAgent, COUNT(FirstAgent) as batman FROM data_df_tot_file WHERE FirstAgent != "" GROUP BY Simulation ORDER BY -batman', con_db)
interactionsCount = pd.read_sql_query('SELECT COUNT(FirstAgent) as total FROM data_df_tot_file', con_db)
interactionsPerPos = pd.read_sql_query('SELECT Simulation, FirstAgent, FirstAgentPosX, FirstAgentPosY, FirstAgentO, FirstAgentC, FirstAgentE, FirstAgentA, FirstAgentN, COUNT(FirstAgent) as countFirstAgents FROM data_df_tot_file GROUP BY FirstAgentPosX, FirstAgentPosY', con_db)
allFirstAndSecondAgents = pd.read_sql_query('SELECT FirstAgent, Frame, SecondAgent, FirstAgentO, FirstAgentC, FirstAgentE, FirstAgentA, FirstAgentN, SecondAgentO, SecondAgentC, SecondAgentE, SecondAgentA, SecondAgentN, Simulation FROM data_df_tot_file', con_db)
dfAllFirstAndSecondAgents = pd.DataFrame(allFirstAndSecondAgents)
dfcountsimulations = pd.DataFrame(countSimulations)
totalInteractionsPerSimulation = pd.read_sql_query('SELECT Simulation, FirstAgent, COUNT(FirstAgent) as countFirstAgents FROM data_df_tot_file GROUP BY Simulation', con_db)
totalInteractions = pd.read_sql_query('SELECT Simulation, FirstAgent, COUNT(FirstAgent) as countFirstAgents FROM data_df_tot_file GROUP BY Simulation', con_db)
simulationsPerPos = pd.read_sql_query('SELECT Simulation, FirstAgentPosX, FirstAgentPosY, SecondAgentPosX, SecondAgentPosY, COUNT(FirstAgent) as count FROM data_df_tot_file GROUP BY Simulation', con_db)
posXZFirstAgent = pd.read_sql_query('SELECT FirstAgentPosX, FirstAgentPosY, FirstAgent, SecondAgent, Simulation, COUNT(FirstAgent) as countFirstAgents FROM data_df_tot_file  GROUP BY FirstAgent', con_db)
frameSimulations = pd.read_sql_query('SELECT COUNT(FirstAgent) as countFirstAgents, Simulation, Frame, FirstAgent FROM data_df_tot_file GROUP BY Frame, Simulation', con_db)
dfFrameSimulations = pd.DataFrame(frameSimulations)
interactions = pd.read_sql_query('SELECT FirstAgent, SecondAgent, Simulation FROM data_df_tot_file GROUP BY SecondAgent', con_db)
optionsList = []

#print(test1)

'''columns_person_elements_video_file = sqlite3.connect("columns_person_elements_video_file.db")
cur_columns_person_elements_video_file = columns_person_elements_video_file.cursor()
test2 = pd.read_sql_query('SELECT * FROM columns_person_elements_video_file', columns_person_elements_video_file)
print(test2)'''

'''columns_total_elements_file = sqlite3.connect("columns_total_elements_file.db")
cur_columns_total_elements_file= columns_total_elements_file.cursor()
test3 = pd.read_sql_query('SELECT * FROM columns_total_elements_file', columns_total_elements_file)
print(test3)'''

for i in simulationsPerPos.Simulation:
    optionsList.append(i)
optionsList.append('None')



app.layout = html.Div([
        html.Div(
            dcc.Tabs(
                tabs=[
                    {'label': 'VIDEOS X INTERACTIONS', 'value': 1},
                    {'label': 'INTERACTIONS X LOCATIONS', 'value': 3},
                    {'label': 'RELATIONSHIPS BETWEEN PEOPLE', 'value': 4},
                    {'label': 'FRAMES X INTERACTIONS', 'value': 2},
                ],
                value=1,
                id='tabs',
                vertical= True,
                style={
                    'fontFamily': 'Arial',
                    'borderRight': 'thin lightgrey solid',
                    'textAlign': 'left',
                    'fontSize': 15,
                    'height': '10%'
                }
            ),
            style={'width': '12%', 'height':'17%', 'float': 'left'}
        ),

        html.Div([dcc.Dropdown(
                id='crossfilter-simulations',
                options=[{'label': i, 'value': i} for i in optionsList],
                value='AE-01'
        )], style={'width': '48%', 'fontFamily': 'Arial', 'display': 'inline-block'}),

        html.Div(
            html.Div(id='graph-output'),
            style={'width': '80%', 'float': 'right'}
        )
        
])

@app.callback(Output('graph-output', 'children'), [Input('tabs', 'value'), Input('crossfilter-simulations', 'value')])
def display_content(valueTab, valueOptions):
    data = []
    layout = []
    testEdges = []
    testNodes = []
    
    if (valueTab == 1):
        
        data = [
            {
                'x': countSimulations.Simulation,
                'y': round((countSimulations.batman/2), 4),
                'text': countSimulations.batman/2,
                'hoverinfo': 'text',
                'marker': {
                    'color': 'rgb(55, 83, 109)'
                },
                'type': 'bar'
            }
        ]

        layout = go.Layout(
            annotations=[
                dict(
                    x=0.5004254919715793,
                    y=-0.33191064079952971,
                    showarrow=False,
                    text='VIDEOS',
                    xref='paper',
                    yref='paper'
                ),
                dict(
                    x=-0.07944728761514841,
                    y=0.4714285714285711,
                    showarrow=False,
                    text='COUNT INTERACTIONS',
                    textangle=-90,
                    xref='paper',
                    yref='paper'
                )
            ],
            title='COUNT INTERACTIONS x VIDEOS',
            width=950,
            height=550,
            legend=dict(x=-.1, y=1.2)
        )
    if(valueTab == 4 and valueOptions != 'None'):
        counttotalsimulation = dfcountsimulations[dfcountsimulations['Simulation'] == valueOptions]
        #print(counttotalsimulation)
        dfTestSimulationFirstAgent = dfAllFirstAndSecondAgents[dfAllFirstAndSecondAgents['Simulation'] == str(valueOptions)]
        interactionsO = []
        interactionsC = []
        interactionsE = []
        interactionsA = []
        interactionsN = []
        allColors = []
        allFrames = []
        allOCEANtext = []
        allnewTexts = []
        listAdjacencies = []
        newCountFirstAgent = []
        
        GTest = nx.Graph()
        
        for i, o, c, e, a, n in zip(dfTestSimulationFirstAgent['FirstAgent'], dfTestSimulationFirstAgent['FirstAgentO'], dfTestSimulationFirstAgent['FirstAgentC'], dfTestSimulationFirstAgent['FirstAgentE'], dfTestSimulationFirstAgent['FirstAgentA'], dfTestSimulationFirstAgent['FirstAgentN']):
            
            if((i not in GTest.nodes() or len(GTest.nodes()) == 0) and i != None):
                interactionsO.append(o)
                interactionsC.append(c)
                interactionsE.append(e)
                interactionsA.append(a)
                interactionsN.append(n)
                GTest.add_node(i)

        interactionsFactorList = []
        for i in range(len(GTest.nodes())):
            interactionsFactorList.append((0.45 * interactionsO[i]) + (0.05 * (1-interactionsC[i])) + (0.45 * interactionsE[i]) + (0.05 * interactionsA[i]) + (0.05 * (1 - interactionsN[i])))
            if((interactionsO[i] <= 0.5 or interactionsO[i] > 0.2 or interactionsO[i] < 0.8) and (interactionsC[i] <= 0.5 or interactionsC[i] > 0.2 or interactionsC[i] < 0.8) and (interactionsE[i] > 0.2 or interactionsE[i] <= 0.5 or interactionsE[i] < 0.8) and (interactionsA[i] > 0.2 or interactionsA[i] <= 0.5 or interactionsA[i] < 0.8) and (interactionsN[i] > 0.2 or interactionsN[i] <= 0.5 or interactionsN[i] < 0.8)):
                allColors.append('rgb(190,190,190)')
                allOCEANtext.append('Neutral Personality')
            elif(interactionsO[i] <= 0.2 and interactionsC[i] <= 0.2 and interactionsE[i] <= 0.2 and interactionsA[i] <= 0.2 and interactionsN[i] >= 0.8):
                allColors.append('rgb(0,0,255)')
                allOCEANtext.append('Blue Personality')
            elif(interactionsO[i] == 0.8 and interactionsC[i] >= 0.8 and interactionsE[i] >= 0.8 and interactionsA[i] >= 0.8 and interactionsN[i] <= 0.2):
                allColors.append('rgb(255,192,203)')
                allOCEANtext.append('Pink Personality')

        secondagentlist = []
        countsecondagentlist = []
        countsecondagentliststring = []
        for i in GTest.nodes():
            countsecondagent = 0
            for j, k, f in zip(dfTestSimulationFirstAgent['FirstAgent'], dfTestSimulationFirstAgent['SecondAgent'], dfTestSimulationFirstAgent['Frame']):
                if(i == j and k != None):
                    if i != k:
                        secondagentlist.append((i, k))
                        #print((i, k))
                        countsecondagent += 1
                    allFrames.append(f)
                    GTest.add_edge(i, k) 
            countsecondagentlist.append(countsecondagent)
            countsecondagentliststring.append(countsecondagent/10)
            #print(countsecondagent/50)
        countAdjacencies = 0
        for node, adjacencies in enumerate(GTest.adjacency_list()):
            listAdjacencies.append(len(adjacencies))
        posGTest = nx.fruchterman_reingold_layout(GTest)

        
        Xn=[posGTest[k][0] for k in GTest.nodes()]
        Yn=[posGTest[k][1] for k in GTest.nodes()]
        
        for i in range(len(GTest.nodes())):
            allnewTexts.append(str(GTest.nodes()[i]) + " CountRelationships:" + str(listAdjacencies[i]) + " CountInteractions:" + str(countsecondagentlist[i]) + " O:" + str(round(interactionsO[i], 2)) + " C:" + str(round(interactionsC[i], 2)) + " E:" + str(round(interactionsE[i], 2)) + " A:" + str(round(interactionsA[i], 3)) + " N:" + str(round(interactionsN[i], 3)) + " InteractionFactor:" + str(round(interactionsFactorList[i], 2)))


        labels = GTest.nodes()
        Xe=[]
        Ye=[]
        for e in GTest.edges():
            Xe+=[posGTest[e[0]][0],posGTest[e[1]][0], None]
            Ye+=[posGTest[e[0]][1],posGTest[e[1]][1], None]

        trace3=go.Scatter(x=Xe,
                    y=Ye,
                    mode='lines',
                    line=go.scatter.Line(color='rgb(210,210,210)', width=1),
                    text=allFrames,
                    showlegend=True,
                    name='Relationships Lines',
                    hoverinfo='text'
                    )
        trace4=go.Scatter(x=Xn,
                    y=Yn,
                    mode='markers',
                    name='People',
                    marker=go.scatter.Marker(#symbol='dot',
                                    #size=listAdjacencies,
                                    size=countsecondagentliststring, 
                                    color=allColors,
                                    line=go.scatter.marker.Line(color='rgb(50,50,50)', width=0.5)
                                    ),
                    text=allnewTexts,
                    showlegend=True,
                    hoverinfo='text'
                    )
        axis=dict(showline=False,
                zeroline=False,
                showgrid=False,
                showticklabels=False,
                title='' 
                )
        width=950
        height=550
        layout = go.Layout(title= "PERSONAL RELATIONSHIPS",  
            font= go.layout.Font(size=12),
            showlegend=True,
            legend=dict(x=-.1, y=1.2),
            autosize=False,
            width=width,
            height=height,
            xaxis=go.layout.XAxis(axis),
            yaxis=go.layout.YAxis(axis),          
            margin=go.layout.Margin(
                l=40,
                r=40,
                b=85,
                t=100,
            ),
            hovermode='closest',
            annotations=go.layout.Annotations([
                go.Annotation(
                showarrow=False, 
                    text=str(valueOptions),  
                    xref='paper',     
                    yref='paper',     
                    x=0,  
                    y=-0.1,  
                    xanchor='left',   
                    yanchor='bottom',  
                    font=go.Font(
                    size=14 
                    )     
                    )
                ]),        
            )

        data=go.Data([trace3, trace4])
            
    if(valueTab == 3 and valueOptions != None):
        
        newFirstAgentPosX = []
        newFirstAgentPosZ = []
        newFirstAgentO = []
        newFirstAgentC = []
        newFirstAgentE = []
        newFirstAgentA = []
        newFirstAgentN = []
        newCountFirstAgent = []
        newColors = []
        newSize = []
        allOCEANtext = []
        allnewTexts = []
        totalInteractionsVariable = 0

        interactionsperpos_test = interactionsPerPos[interactionsPerPos['Simulation'] == valueOptions]
        dfinteractionsperpos = pd.DataFrame(interactionsperpos_test)
        #print(interactionsPerPos)
        #print(dfinteractionsperpos)
        dfinteractionsperposMAX = dfinteractionsperpos['FirstAgentPosY'].max()
        #print(dfinteractionsperpos['FirstAgentPosY'])
        #dfinteractionsperposMAX = 5 
        for i in range(len(interactionsPerPos)):
            if(valueOptions == str(interactionsPerPos.Simulation[i]) and interactionsPerPos.FirstAgent[i] != None):
                newFirstAgentPosX.append(round(interactionsPerPos.FirstAgentPosX[i], 2))
                newFirstAgentPosZ.append(round(dfinteractionsperposMAX - interactionsPerPos.FirstAgentPosY[i], 2))
                #print("Max: " + str(dfinteractionsperposMAX) + " Atual: " + str(interactionsPerPos.FirstAgentPosY[i]))
                newFirstAgentO.append(interactionsPerPos.FirstAgentO[i])
                newFirstAgentC.append(interactionsPerPos.FirstAgentC[i])
                newFirstAgentE.append(interactionsPerPos.FirstAgentE[i])
                newFirstAgentA.append(interactionsPerPos.FirstAgentA[i])
                newFirstAgentN.append(interactionsPerPos.FirstAgentN[i])
        test = 0
        for i in range(len(interactionsPerPos.countFirstAgents)):
            if(valueOptions == str(interactionsPerPos.Simulation[i]) and interactionsPerPos.FirstAgent[i] != None):
                newCountFirstAgent.append(interactionsPerPos.countFirstAgents[i])
            
        valueTotalInteractions = 0
        for i in range(len(totalInteractionsPerSimulation)):
            if(str(totalInteractionsPerSimulation.Simulation[i]) == valueOptions): 
                valueTotalInteractions = totalInteractionsPerSimulation.countFirstAgents[i]

        for j in range(len(newFirstAgentPosX)):
            divisionPerPorc = 12*((newCountFirstAgent[j]*100)/valueTotalInteractions)
            newSize.append(divisionPerPorc)
            

        for i in range(len(totalInteractions)):
            if(str(totalInteractions.Simulation[i]) == valueOptions and totalInteractions.FirstAgent[i] != None):
                totalInteractionsVariable = totalInteractions.countFirstAgents[i]

        for i in range(len(newFirstAgentPosX)):
            '''if(newFirstAgentO[i] == 0.5 and newFirstAgentC[i] == 0.5 and newFirstAgentE[i] == 0.5 and newFirstAgentA[i] == 0.5 and newFirstAgentN[i] == 0.5):
                newColors.append('rgb(190,190,190)')
                allOCEANtext.append('Neutral Personality')
            elif(newFirstAgentO[i] == 0.2 and newFirstAgentC[i] == 0.2 and newFirstAgentE[i] == 0.2 and newFirstAgentA[i] == 0.2 and newFirstAgentN[i] == 0.8):
                newColors.append('rgb(0,0,255)')
                allOCEANtext.append('Blue Personality')
            elif(newFirstAgentO[i] == 0.8 and newFirstAgentC[i] == 0.8 and newFirstAgentE[i] == 0.8 and newFirstAgentA[i] == 0.8 and newFirstAgentN[i] == 0.2):
               ''' 
            newColors.append('rgb(0,0,0)')
                #allOCEANtext.append('Pink Personality')
        
        for i in range(len(newFirstAgentPosX)):
            allnewTexts.append(" Position=(" + str(newFirstAgentPosX[i]) + "," + str(newFirstAgentPosZ[i]) + ") InteractionsCount: " + str(newCountFirstAgent[i]))


        data = [
            go.Scatter(
                x=newFirstAgentPosX,
                y= newFirstAgentPosZ,
                mode='markers',
                marker=dict(
                    size=newCountFirstAgent,
                    color = newColors,
                    colorscale='Viridis',
                    showscale=False
                ),
                hoverinfo='text',
                text=allnewTexts
            )
    ]

        layout = go.Layout(
            title='VIDEO INTERACTIONS x POSITIONS',
            xaxis = dict(ticks='', nticks=36),
            width=950,
            height=550,
            yaxis = dict(ticks='' ),
            annotations=[
                dict(
                    x=0.5004254919715793,
                    y=-0.13191064079952971,
                    showarrow=False,
                    text='X AXIS',
                    xref='paper',
                    yref='paper'
                ),
                dict(
                    x=-0.07944728761514841,
                    y=0.4714285714285711,
                    showarrow=False,
                    text='Y AXIS',
                    textangle=-90,
                    xref='paper',
                    yref='paper'
                )
            ]
        )

    if(valueTab == 2 and valueOptions != None):

        frameList = []
        countInteractionsList = []

        for i in range(len(frameSimulations)):
            if(str(frameSimulations.Simulation[i]) == str(valueOptions) and frameSimulations.FirstAgent[i] != None):
                frameList.append(frameSimulations.Frame[i])
                countInteractionsList.append(frameSimulations.countFirstAgents[i]/2)
               
        trace_high = go.Scatter(
            x=frameList,
            y=countInteractionsList,
            line = dict(color = '#17BECF'),
            opacity = 0.8)

        data = [trace_high]

        layout = go.Layout(
                    title='FRAMES x COUNT INTERACTIONS',
                    annotations=[
                                dict(
                                    x=0.5004254919715793,
                                    y=-0.43191064079952971,
                                    showarrow=False,
                                    text='FRAMES',
                                    xref='paper',
                                    yref='paper'
                                ),
                                dict(
                                    x=-0.07944728761514841,
                                    y=0.4714285714285711,
                                    showarrow=False,
                                    text='COUNT INTERACTIONS',
                                    textangle=-90,
                                    xref='paper',
                                    yref='paper'
                                )
                            ],
                    xaxis=dict(
                        rangeselector=dict(
                            buttons=list([
                                dict(count=1,
                                    label='1m',
                                    stepmode='backward'),
                                dict(count=6,
                                    label='6m',
                                    stepmode='backward'),
                                dict(count=1,
                                    label='YTD',
                                    stepmode='todate'),
                                dict(count=1,
                                    label='1y',
                                    stepmode='backward'),
                                dict(step='all')
                            ])
                        ),
                        rangeslider=dict()
                    ),
                    width=950,
                    height=550
                )

    return html.Div([
        dcc.Graph(
            id='graph',
            figure={
                'data': data,
                'layout': layout
            },
            config={
                'displayModeBar': False
            },
        style={'width': 1000, 'height':700}
        )
    ])

if __name__ == '__main__':
    app.run_server()
