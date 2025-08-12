<template>
  <div class="form-overlay">
    <div class="form-container">
      <h3>{{ student?.id ? '✏️ Editar Aluno' : '➕ Cadastrar Aluno' }}</h3>

      <form @submit.prevent="save">
        <label>Nome:</label>
        <input v-model="form.name" required />

        <label>Email:</label>
        <input v-model="form.email" required />

        <label>RA:</label>
        <input v-model="form.ra" required />

        <label>CPF:</label>
        <input v-model="form.cpf" required />

        <div class="form-actions">
          <button type="submit">💾 Salvar</button>
          <button type="button" @click="$emit('close')">❌ Cancelar</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import type { Student } from '../types/Student'
import { createStudent, updateStudent } from '../services/studentService'

const props = defineProps<{ student: Student | null }>()
const emit = defineEmits(['close', 'saved'])

const form = ref<Student>
({
  id: 0,
  name: '',
  email: '',
  ra: '',
  cpf: ''
})

watch(() => props.student, (newVal) => {
  form.value = newVal
    ? { ...newVal }
    : { id: 0, name: '', email: '', ra: '', cpf: '' }
}, { immediate: true })

async function save() {
  if (form.value.id) {
    await updateStudent(form.value.id, form.value)
  } else {
    await createStudent(form.value)
  }
  emit('saved')
  emit('close')
}
</script>

<style scoped>
.form-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.form-container {
  background: white;
  padding: 1rem;
  border-radius: 8px;
  width: 300px;
}

label {
  display: block;
  margin-top: 0.5rem;
}

input {
  width: 100%;
  padding: 0.4rem;
  margin-bottom: 0.5rem;
}

.form-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 1rem;
}
</style>