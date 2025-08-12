<template>
  <div class="student-list">
    <header class="header">
      <h2>📚 Listagem de Alunos</h2>
      <div class="actions">
        <input v-model="search" placeholder="🔍 Buscar por nome..." />
        <button @click="openForm()">➕ Cadastrar Aluno</button>
      </div>
    </header>

    <table class="student-table">
      <thead>
        <tr>
          <th>RA</th>
          <th>Nome</th>
          <th>CPF</th>
          <th>Ações</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="student in filteredStudents" :key="student.id">
          <td>{{ student.ra }}</td>
          <td>{{ student.name }}</td>
          <td>{{ student.cpf }}</td>
          <td>
            <button @click="openForm(student)">✏️ Editar</button>
            <button @click="deleteStudent(student.id)">🗑️ Excluir</button>
          </td>
        </tr>
        <tr v-if="filteredStudents.length === 0">
          <td colspan="4">Nenhum aluno encontrado.</td>
        </tr>
      </tbody>
    </table>

    <!-- Temporariamente removido para evitar erro -->
     <StudentForm
      v-if="showForm"
      :student="selectedStudent"
      @close="closeForm"
      @saved="refreshList"
    />
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, onMounted } from 'vue'
 import StudentForm from '../components/StudentForm.vue'
import { getAllStudents, deleteStudentById } from '../services/studentService'

import type { Student } from '../types/Student'

const students = ref<Student[]>([])
const search = ref('')
const showForm = ref(false)
const selectedStudent = ref<Student | null>(null)

const filteredStudents = computed(() =>
  students.value.filter(s =>
    s.name.toLowerCase().includes(search.value.toLowerCase()) ||
    s.ra.toLowerCase().includes(search.value.toLowerCase()) ||
    s.cpf.toLowerCase().includes(search.value.toLowerCase())
  )
)

function openForm(student: Student | null = null) {
  selectedStudent.value = student
  showForm.value = true
}

function closeForm() {
  selectedStudent.value = null
  showForm.value = false
}

async function deleteStudent(id: number) {
  students.value = students.value.filter(s => s.id !== id)
}

async function refreshList() {
  // Dados mockados para garantir funcionamento
  try {
    students.value = await getAllStudents()
  } catch (error) {
    console.error('Erro ao buscar alunos:', error)
  }


}

onMounted(refreshList)
</script>

<style scoped>
.student-list {
  padding: 1rem;
  max-width: 800px;
  margin: auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.actions {
  display: flex;
  gap: 0.5rem;
}

.student-table {
  width: 100%;
  border-collapse: collapse;
}

.student-table th,
.student-table td {
  border: 1px solid #ccc;
  padding: 0.5rem;
  text-align: left;
}

.student-table th {
  background-color: #f0f0f0;
}

button {
  margin-right: 0.5rem;
  cursor: pointer;
}
</style>