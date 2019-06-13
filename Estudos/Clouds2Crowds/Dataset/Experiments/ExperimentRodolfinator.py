import numpy as np
import sys
from math  import sqrt

def euclid(a, b):
    diff = (a[0] - b[0], a[1] - b[1])
    return sqrt(diff[0]*diff[0] + diff[1]*diff[1])

class AgentData():
    def __init__(self, id,  cloudID, firstFrame):
        self.id = id
        self.cloudID = cloudID
        self.startframe = firstFrame
        self.positions = {}

# Parse Agents
agentsInFrame = {}

agentDict = {}
agentsFileName = sys.argv[1]
agentsFile = open(agentsFileName)

for line in agentsFile:
    if(line.startswith("#")):
        continue
    s_line = line.split(";")
    frame = int(s_line[0])
    count = int(s_line[1])

    agentsInFrame[frame] = []

    for i in range(count):
        idx = i*4 + 2
        a_id = int(s_line[idx])
        cloud_id = int(s_line[idx+1])

        #print(frame, count, idx, a_id, cloud_id)

        if(a_id not in agentDict):
            agentDict[a_id] = AgentData(a_id, cloud_id,  frame)

        agentDict[a_id].positions[frame] = (float(s_line[idx+2].replace(",",".")), float(s_line[idx+3].replace(",",".")))

        agentsInFrame[frame].append(a_id)


rodolfinatedAgents = open(sys.argv[1].split("\\")[-1].split(".")[-2] + "rodolfinatedAgents.txt", 'w')

for i in agentDict:
	agent = agentDict[i]
	rodolfinatedAgents.write("P-" + str(agent.id) + "\n")
	
	currentFrame = agent.startframe
	
	while currentFrame in agent.positions:
		
		position = agent.positions[currentFrame]
		rodolfinatedAgents.write(str(currentFrame+1) + " " + str(int(position[0] * 100)) + " " + str(int(position[1] * 100)) + "\n")
		
		currentFrame+=1

