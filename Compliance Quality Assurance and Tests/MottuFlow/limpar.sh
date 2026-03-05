#!/bin/bash
set -euo pipefail

RG="sprint-mottuflow"

echo "⚠️ Deletando o Resource Group $RG e todos os recursos associados..."
az group delete --name "$RG" --yes --no-wait

echo "🗑️ $RG está sendo deletado!"