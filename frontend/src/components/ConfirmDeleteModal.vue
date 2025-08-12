<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <p>{{ message }}</p>
      <div class="modal-actions">
        <button @click="$emit('confirm')">Sim, excluir</button>
        <button @click="$emit('cancel')">Cancelar</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  message: string
}>()
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0; left: 0;
  width: 100%; height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex; justify-content: center; align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 24px;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.2);
  text-align: center;
}

.modal-actions {
  margin-top: 16px;
  display: flex;
  justify-content: center;
  gap: 12px;
}

button {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:first-child {
  background-color: #e53935;
  color: white;
}

button:last-child {
  background-color: #ccc;
}
</style>