#!/bin/bash

# 🔧 Azure Health Check Script for LindebergsHealth
# Performs quick health checks using Azure MCP Server

set -e

echo "🏥 LindebergsHealth - Azure Health Check"
echo "========================================"

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Function to print status
print_status() {
    if [ $2 -eq 0 ]; then
        echo -e "${GREEN}✅ $1${NC}"
    else
        echo -e "${RED}❌ $1${NC}"
    fi
}

# Check Azure CLI Authentication
echo -e "${YELLOW}🔐 Checking Azure Authentication...${NC}"
if az account show &> /dev/null; then
    SUBSCRIPTION=$(az account show --query name -o tsv)
    print_status "Azure CLI authenticated - Subscription: $SUBSCRIPTION" 0
else
    print_status "Azure CLI not authenticated" 1
    echo "Run 'az login' to authenticate"
    exit 1
fi

# Check Node.js/npm for Azure MCP Server
echo -e "${YELLOW}📦 Checking Node.js/npm...${NC}"
if command -v node &> /dev/null && command -v npm &> /dev/null; then
    NODE_VERSION=$(node --version)
    NPM_VERSION=$(npm --version)
    print_status "Node.js $NODE_VERSION, npm $NPM_VERSION available" 0
else
    print_status "Node.js/npm not available" 1
    exit 1
fi

# Test Azure MCP Server availability
echo -e "${YELLOW}🌐 Testing Azure MCP Server...${NC}"
if npx -y @azure/mcp@latest server start --help &> /dev/null; then
    print_status "Azure MCP Server available" 0
else
    print_status "Azure MCP Server not available" 1
fi

# Check LindebergsRG Resource Group
echo -e "${YELLOW}🏥 Checking LindebergsHealth Resources...${NC}"
if az group show --name LindebergsRG &> /dev/null; then
    LOCATION=$(az group show --name LindebergsRG --query location -o tsv)
    print_status "LindebergsRG Resource Group found in $LOCATION" 0
else
    print_status "LindebergsRG Resource Group not found" 1
fi

# Check for Web Apps in Resource Group
echo -e "${YELLOW}🚀 Checking Web Applications...${NC}"
WEBAPPS=$(az webapp list --resource-group LindebergsRG --query "[].name" -o tsv 2>/dev/null | wc -l | tr -d ' ')
if [ "$WEBAPPS" -gt 0 ]; then
    print_status "$WEBAPPS Web App(s) found in LindebergsRG" 0
    az webapp list --resource-group LindebergsRG --query "[].{Name:name,State:state,Location:location}" -o table
else
    print_status "No Web Apps found in LindebergsRG" 1
fi

# Summary
echo ""
echo "📋 Health Check Summary"
echo "======================"
echo "Azure Subscription: $SUBSCRIPTION"
echo "Resource Group: LindebergsRG"
echo "MCP Server: Ready for AI-powered development"
echo ""
echo "🤖 Try these AI prompts in GitHub Copilot:"
echo "- 'List all resources in LindebergsRG'"
echo "- 'Show Web App status for LindebergsHealth'"
echo "- 'Check deployment logs for latest release'"

exit 0 