<template>
  <div>
    <h3>{{ student?.id ? 'Editar Aluno' : 'Criar Aluno' }}</h3>
    <form @submit.prevent="save">
      <input v-model="form.name" placeholder="Nome" required />
      <input v-model="form.email" placeholder="Email" required />
      <input v-model="form.ra" placeholder="RA" required />
      <input v-model="form.cpf" placeholder="CPF" required />

      <button type="submit">Salvar</button>
      <button type="button" @click="$emit('close')">Cancelar</button>
    </form>
  </div>
</template>

<script lang="ts" setup>
import { reactive, watch, toRefs } from 'vue'
import { saveStudent } from '../services/studentService'
import type { Student } from '../types/Student'

const props = defineProps<{ student: Student | null }>()
const emit = defineEmits(['close', 'saved'])

const form = reactive<Student>({
  id: 0,
  name: '',
  email: '',
  ra: '',
  cpf: '',
  isActive: true
})

watch(() => props.student, (newVal) => {
  if (newVal) Object.assign(form, newVal)
  else Object.assign(form, { id: 0, name: '', email: '', ra: '', cpf: '', isActive: true })
}, { immediate: true })

async function save() {
  await saveStudent(form)
  emit('saved')
  emit('close')
}
</script>